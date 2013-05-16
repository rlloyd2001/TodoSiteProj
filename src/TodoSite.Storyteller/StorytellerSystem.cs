using System;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.PersistedMembership;
using FubuMVC.StructureMap;
using Serenity;
using StoryTeller.Engine;
using StructureMap;
using TodoSite.App_Start;

namespace TodoSite.StoryTeller
{
    public class TodoSiteStoryTellerApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IUsersService>().Use<FakeUsersService>();
            });
            var container = ObjectFactory.Container;
            return FubuApplication.For<StoryTellerFubuRegistry>().StructureMap(container);
            throw new System.NotImplementedException();
        }
    }

    public class StoryTellerFubuRegistry : FubuRegistry
    {
        public StoryTellerFubuRegistry()
        {
            Actions.IncludeClassesSuffixedWithController();
        }
    }

    public class StoryTellerSystem : FubuMvcSystem<TodoSiteStoryTellerApplication>
    {
        protected override void configureApplication(IApplicationUnderTest application, BindingRegistry binding)
        {
            var ha = application.GetHashCode();
            WebDriverSettings.Current.Browser = BrowserType.Chrome;
        }
    }
}
