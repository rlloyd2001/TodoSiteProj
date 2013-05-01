unless Gem.available? 'bundler'
  system 'gem install bundler'
end
system 'bundle install'
puts "You're all set, please use 'bundle exec rake'"
