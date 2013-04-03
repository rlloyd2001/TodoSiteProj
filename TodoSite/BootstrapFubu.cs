using System.Web.Routing;
using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap.Configuration.DSL;
using FubuMVC.Spark;
using StructureMap;

namespace TodoSite
{
    // Using a separate class for bootstrapping makes it much easier to reuse your application 
    // in testing scenarios with either SelfHost or OWIN/Katana hosting
    public class MyApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            // This is bootstrapping an application with all default FubuMVC conventions and
            // policies pulling actions from only this assembly for classes suffixed with
            // "Endpoint" or "Endpoints"
            //return FubuApplication.DefaultPolicies().StructureMap<MyStructureMapRegistry>();


            // Fancier way if you want to specify your own policies:
            return FubuApplication.For<MyFubuMvcPolicies>().StructureMap(new Container());


            // Here's an example of using StructureMap specific registration with a StructureMap Registry.  
            //return FubuApplication.For<MyFubuMvcPolicies>().StructureMap<MyStructureMapRegistry>();
        }
    }

    public class MyStructureMapRegistry : Registry
    {
        public MyStructureMapRegistry()
        {
            // StructureMap registration here                
        }
    }

    public class MyFubuMvcPolicies : FubuRegistry
    {
        public MyFubuMvcPolicies()
        {
            // This is a DSL to change or add new conventions, policies, or application settings

            // This line turns on the basic diagnostics and request tracing
            //IncludeDiagnostics(true); (OBSOLETE?)

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();
            
            //Applies (OBSOLETE?)
                //.ToThisAssembly()
                //.ToAllPackageAssemblies();

            // Policies
            Routes
                //.IgnoreControllerNamesEntirely()
                //.IgnoreControllerNamespaceEntirely()
                //.IgnoreMethodSuffix("Html")
                //.ConstrainToHttpMethod(x => x.Method.Name.StartsWith("Edit"), "POST")
                //.ConstrainToHttpMethod(x => x.Method.Name.StartsWith("Add"), "POST")
                //.ConstrainToHttpMethod(x => x.Method.Name.StartsWith("Delete"), "POST")
                .HomeIs<UserLoginController>(x => x.UserLogin());                
                //.RootAtAssemblyNamespace();
            
            /*HtmlConvention(x =>
            {
                x.Editors.IfPropertyIs<DateTime>().BuildBy(
                    request => new TextboxTag().Attr("value", request.Value<DateTime>().ToShortDateString()));
                x.Displays.IfPropertyIs<DateTime>().BuildBy(
                    request => new HtmlTag("span").Text(request.Value<DateTime>().ToShortDateString()));

                x.Editors.IfPropertyIs<Colors>().Modify(
                    (request, tag) => tag.Style("color", request.StringValue()));
                x.Displays.IfPropertyIs<Colors>().Modify(
                    (request, tag) => tag.Style("color", request.StringValue()));
            });*/
        }
    }
}