using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using FubuCore;
using FubuCore.Binding;
using FubuCore.Dates;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Runtime;
using FubuMVC.StructureMap;
using FubuMVC.Validation;
using FubuMVC.Validation.UI;
using FubuPersistence;
using FubuPersistence.InMemory;
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
            var container = new Container();
            return FubuApplication.For<MyFubuMvcPolicies>().StructureMap(container);


            // Here's an example of using StructureMap specific registration with a StructureMap Registry.  
            //return FubuApplication.For<MyFubuMvcPolicies>().StructureMap<MyStructureMapRegistry>();
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

            //Services(x => x.AddService<IUsersService, FakeUsersService>());
            Services(x => x.AddService<IUsersService, RavenDbUsersService>());
            
            Policies.Add<StopWatchPolicy>();

            AlterSettings<ValidationSettings>(validation =>
            {
                validation.ForInputType<UserInputModel>(x =>
                {
                    //x.Clear();
                    x.RegisterStrategy(RenderingStrategies.Inline);
                });
                //validation.ForInputTypesMatching(type => true, x =>
                //{
                //   x.RegisterStrategy(RenderingStrategies.Inline);
                //});
            });

            //Services(x => x.AddService<Registry>(typeof (InMemoryPersistenceRegistry)));
        }
    }

    public class StopWatchPolicy : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Where(x => x.HandlerType.Namespace.StartsWith("TodoSite"))
                .Each(x => x.AddBefore(new StopwatchNode()));
        }

        public void Log(double result)
        {
            Debug.WriteLine(result);
        }
    }


    public class MyStopwatchBehavior : WrappingBehavior
    {
        private readonly IOutputWriter _outputWriter;

        public MyStopwatchBehavior(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        protected override void invoke(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                action();
            }
            finally
            {
                stopwatch.Stop();
                var result = stopwatch.ElapsedMilliseconds;
                _outputWriter.Write("Time: {0}".ToFormat(result));
            }
        }
    }

    public class StopwatchNode : Wrapper
    {
        public StopwatchNode() : base(typeof(MyStopwatchBehavior))
        { }
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