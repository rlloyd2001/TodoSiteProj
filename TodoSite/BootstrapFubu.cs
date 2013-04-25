using System;
using System.Reflection;
using System.Web.Routing;
using Bottles;
using FubuCore.Binding;
using FubuCore.Dates;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
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
            
            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();
            
            // Policies
            Routes
                .HomeIs<UserLoginController>(x => x.Home());
            Models
                .BindPropertiesWith<CurrentTimePropertyBinder>();
        }
    }
    
    public class CurrentTimePropertyBinder : IPropertyBinder
    {
        private readonly ISystemTime _systemTime;

        public CurrentTimePropertyBinder(ISystemTime systemTime)
        {
            _systemTime = systemTime;
        }

        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType == typeof(DateTime)
                && property.Name == "CurrentTime";
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var value = _systemTime.UtcNow().ToLocalTime();
            property.SetValue(context.Object, value, null);
        }
    }
}