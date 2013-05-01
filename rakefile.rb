require 'bundler/setup'

DROP_TARGET = ENV['config'].nil? ? "Debug" : ENV['config'] # Keep this in sync w/ VS settings since Mono is case-sensitive
CLR_TOOLS_VERSION = "v4.0.30319"

include FileTest
require 'albacore'

RESULTS_DIR = "results"
PRODUCT = "TodoSite"
COPYRIGHT = 'Copyright 2012 Extend Health Inc 2012. All rights reserved.';
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
BUILD_DIR = File.expand_path("build")
ARTIFACTS = File.expand_path("artifacts")
MAIN_PROJECT = 'TodoSite.Web'
PACKAGE_DIR = File.join('src', 'packages')

DROP_OPTIONS = {
  project: MAIN_PROJECT,
  target: DROP_TARGET,
  folders: %w{content Shared Endpoints},
  file_types: %w{config asax css js spark htm html zip xml png jpg gif coffee less txt},
  keep_folders: %w{logs}
}


tc_build_number = ENV["BUILD_NUMBER"]
build_revision = tc_build_number || Time.new.strftime('5%H%M')

props = { :stage => BUILD_DIR, :artifacts => ARTIFACTS }

unless (ENV['IGNORE_BUILDSUPPORT'])
  puts "Loading buildsupport"
  buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]

  if(!buildsupportfiles.any?)
    # no buildsupport, let's go get it for them.
    sh 'git submodule update --init' unless buildsupportfiles.any?
    buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]
  end

  # nope, we still don't have buildsupport. Something went wrong.
  raise "Run `git submodule update --init` to populate your buildsupport folder." unless buildsupportfiles.any?

  buildsupportfiles.each { |ext| load ext }
  load "VERSION.txt"
  BUILD_NUMBER = "#{BUILD_VERSION}.#{build_revision}"
end


desc "**Default**, compiles and runs tests"
task :default => %w{compile:debug myNewTask}

task :myNewTask do
  puts 'hello'
end

desc "Update the version information for the build"
assemblyinfo :version do |asm|
  asm_version = BUILD_VERSION + ".0"

  begin
    commit = `git log -1 --pretty=format:%H`
  rescue
    commit = "git unavailable"
  end
  puts "##teamcity[buildNumber '#{BUILD_NUMBER}']" unless tc_build_number.nil?
  puts "Version: #{BUILD_NUMBER}" if tc_build_number.nil?
  asm.trademark = commit
  asm.product_name = PRODUCT
  asm.description = BUILD_NUMBER
  asm.version = asm_version
  asm.file_version = BUILD_NUMBER
  asm.custom_attributes :AssemblyInformationalVersion => asm_version
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO
end

desc "Sets up the Bottles/Fubu aliases"
task :aliases => [:restore_if_missing] do
  #bottles 'alias scenarios src/TodoSite.StoryTeller'
  #bottles 'alias portal src/TodoSite.Web'
end

desc "Prepares the working directory for a new build"
task :clean => [:update_buildsupport] do
  FileUtils.rm_rf props[:stage]
  # work around nasty latency issue where folder still exists for a short while after it is removed
  # waitfor { !exists?(props[:stage]) }
  Dir.mkdir props[:stage]

  Dir.mkdir props[:artifacts] unless exists?(props[:artifacts])
end

def waitfor(&block)
  checks = 0
  until block.call || checks >20
    sleep 0.5
    checks += 1
  end
  raise 'waitfor timeout expired' if checks > 20
end

task :bottle_watcher do
   #bottles 'assembly-pak src/TodoSite.ErrorWatcher'
end

namespace :compile do
  desc "Compiles the app in debug mode"
  task :debug => [:aliases, :bottle_watcher, :clean, :version] do
    compile "Debug"
  end

  desc "Compiles the app in release mode"
  task :release => [:aliases, :bottle_watcher, :clean, :version] do
    compile "Release"
  end

  desc "Compiles the app in all modes"
  task :all => %w{compile:release compile:debug}

  def compile(mode)
    MSBuildRunner.compile :compilemode => mode, :solutionfile => 'src/TodoSite.sln', :clrversion => CLR_TOOLS_VERSION
  end
end

def copyOutputFiles(fromDir, filePattern, outDir)
  Dir.glob(File.join(fromDir, filePattern)) do |file|
    copy(file, outDir, :preserve => true) if File.file?(file)
  end
end
def bottleRunner(assembly)
  sourcePath = File.join('src', assembly, 'bin/debug')
	toolDir = self.tool_dir("BottleServiceRunner")
    
	copyOutputFiles(toolDir, "Bottles.Services.*", sourcePath)
	copyOutputFiles(toolDir, "BottleServiceRunner.exe", sourcePath)
    copyOutputFiles(toolDir, "BottleServiceRunner.pdb", sourcePath)
	copyOutputFiles(toolDir, "TopShelf.*", sourcePath)
	
  executePath = File.join(sourcePath, 'BottleServiceRunner.exe')
  sh "#{executePath}"
end


desc "Un-links the StoryTeller/Scenarios bottle"
task :unlink_scenarios => [:aliases] do
  #bottles 'link portal --clean-all'
  #Rake::Task["restart"].invoke
end

#desc "Runs the StoryTeller UI"
#task :run_st => [:link_scenarios] do
#  st = Platform.runtime(Nuget.tool("Storyteller2", "StorytellerUI.exe"))
#  sh st
#end

#namespace :st do
#  desc "Runs ETL StoryTeller tests"
#  task :etl => [:link_scenarios, :restart] do
#    run_st 'ETL'
#  end
  
#  desc "Runs Backend StoryTeller tests"
#  task :backend => [:link_scenarios, :restart] do
#    run_st 'Backend'
#  end

#  desc "Runs Records StoryTeller tests"
#  task :records => [:link_scenarios, :restart] do
#    run_st 'Records'
#  end

#  def run_st(workspace)
#    storyteller "run src/TodoSite.StoryTeller/portal.xml results/Storyteller.html -w #{workspace}"
#  end
#end

def self.nugetDir(package)
  Dir.glob(File.join(Nuget.package_root,"#{package}.*")).sort.last
end

#desc "Opens the Serenity Jasmine Runner in interactive mode"
#task :open_jasmine => [:setup_jasmine] do
#  serenity "jasmine interactive src/serenity.txt -b Firefox"
#end

desc "Runs the Jasmine tests"
task :run_jasmine => [:setup_jasmine] do
  serenity "jasmine run --timeout 15 src/serenity.txt -b Phantom"
end

#desc "Runs the Jasmine tests and outputs the results for CI"
#task :run_jasmine_ci => [:setup_jasmine] do
#  serenity "jasmine run --verbose --timeout 15 src/serenity.txt -b Phantom"
#end

#desc "Restarts the app"
#task :restart do
#  fubu("restart portal")
#end

desc "Merges the localization files"
task :merge_localization do
    localizer("merge src/TodoSite.Web")
end

def self.serenity(args)
  serenity = 'src/packages/Serenity/tools/SerenityRunner.exe'
  sh "#{serenity} #{args}"
end

def self.localizer(args)
  localizer = Platform.runtime(Nuget.tool("FubuLocalization", "localizer.exe"))
  sh "#{localizer} #{args}"
end

def self.bottles(args)
  bottles = 'src/packages/Bottles/tools/BottleRunner.exe'
  sh "#{bottles} #{args}"
end

def self.raven(args)
  raven = Platform.runtime(Nuget.tool("RavenDB.Server", "Raven.Server.exe"))
  sh "start #{raven} #{args}"
end

def self.fubu(args)
  fubu = Platform.runtime(Nuget.tool("fubu", "fubu.exe"))
  sh "#{fubu} #{args}"
end

#desc "runs the Raven server directly from your packages/RavenDb.Server folder"
#task :run_raven do
#  raven('')
#end

def self.storyteller(args)
  st = Platform.runtime(Nuget.tool("Storyteller2", "st.exe"))
  sh "#{st} #{args}"
end

task :setup_jasmine do
  #jqueryUI = File.join(nugetDir("FubuMVC.JQueryUI"), "lib", "net40", "FubuMVC.JQueryUI.dll")

  bottlesDir = 'src/packages/Serenity/tools'

  #Dir.mkdir bottlesDir unless exists?(bottlesDir)
  #FileUtils.cp_r(jqueryUI, File.join(bottlesDir, "FubuMVC.JQueryUI.dll"))
end

namespace :prepare do
  desc "Prepares web build folder for publish"
  task :web do
    prepare 'Web' do |h|
        artifacts_destination = h[:destination_base_dir]
       
        
        %w{Debug Release}.each do |target|
            destination = File.join(artifacts_destination, target)
            Dir.mkdir destination unless exists?(destination)
            
            bottles_dir = File.join(destination, 'fubu-content')
            Dir.mkdir bottles_dir unless exists?(bottles_dir)
        end
    end
  end

  #TODO -- handle multiple bottles for a given bottle service runner
#  desc "Prepares service build folder for publish"
#  task :service do
#    project = 'TodoSite.ErrorWatcher'
#    drop_for_project project do
#      prepare 'Service' do |h|
#        artifacts_destination = h[:artifacts_destination]

        #Dir.glob(File.join(artifacts_destination, "#{project}.*")) { |file| FileUtils.mv file, artifacts_destination }
#        toolDir = self.tool_dir("BottleServiceRunner")
#        copyOutputFiles(toolDir, "Bottles.Services.*", artifacts_destination)
#        copyOutputFiles(toolDir, "BottleServiceRunner.*", artifacts_destination)
#        copyOutputFiles(toolDir, "TopShelf.*", artifacts_destination)
#      end
#    end
#  end
  
  def self.package_root
    root = nil
    
    packroots = Dir.glob("{source,src}/packages")

    return packroots.last if packroots.length > 0

    Dir.glob("{source,src}").each do |d|
      packroot = File.join d, "packages"
      FileUtils.mkdir_p(packroot) 
      root = packroot
    end       

    root
  end
  
  def self.tool_dir(package)
    File.join('src', 'packages', package, 'tools')
  end
  

  def prepare(drop_folder)
    %w{Debug Release}.each do |target|
      Drop.prepare_dir target, drop_folder do |h|
        Drop.copy_environment_files h[:source_dir], h[:destination_base_dir]
        yield h if block_given?
      end
    end
  end

  #TODO -- update eh build support to handle multiple prepares/publishes
  def drop_for_project(project)
    original_project = DROP_OPTIONS[:project]
    DROP_OPTIONS[:project] = project
    yield
    DROP_OPTIONS[:project] = original_project
  end
end

namespace :publish do
  desc "Publish web build folder for deploy"
  task :web do
    publish_for_prepared_folder 'Web'
  end

  desc "Publish service build folder for deploy"
  task :service do
    publish_for_prepared_folder 'Service'
  end

  def publish_for_prepared_folder(prepared_folder)
    source_path = File.join DROP_BUILD_DIR, prepared_folder
    destination_path = File.join DROP_PUBLISH_DIR, prepared_folder

    rm_rf destination_path if File.exists?(destination_path)

    contents = File.join source_path, DROP_TARGET
    copy_dir(//, contents, destination_path)
    
    return if BUILD_ENV == "Local"

    config_dir = File.join(source_path, DEFAULT_ENV_DIR, BUILD_ENV)
    copy_dir(/^.*\.config$/, config_dir, destination_path)
  end
end

