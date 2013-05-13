using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Serenity;
using StructureMap;

namespace TodSite.Storyteller
{
    public class TodoSiteStorytellerApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication.For<StorytellerFubuRegistry>().StructureMap(new Container());
        }
    }

    public class StorytellerFubuRegistry : FubuRegistry
    {
        public StorytellerFubuRegistry()
        {
            //Import<PersistedMembership<User>>();
        }
    }

    public class StorytellerSystem : FubuMvcSystem<TodoSiteStorytellerApplication>
    {
        protected override void configureApplication(IApplicationUnderTest application, BindingRegistry binding)
        {
            WebDriverSettings.Current.Browser = BrowserType.Chrome;
        }
    }
}