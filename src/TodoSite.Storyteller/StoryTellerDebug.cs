using System;
using NUnit.Framework;
using StoryTeller.Execution;

namespace StoryTellerTestHarness
{
    [TestFixture, Explicit]
    public class Template
    {
        private ProjectTestRunner runner;

        [TestFixtureSetUp]
        public void SetupRunner()
        {
            runner = new ProjectTestRunner(@"C:\home\study\TodoSiteProj\src\TodoSite.Storyteller\storyteller.xml");
            runner.Project.TimeoutInSeconds = 240;
        }

        [Test]
        public void Log_in_successfully()
        {
            runner.RunAndAssertTest("Logins/Log in successfully");
        }

        [TestFixtureTearDown]
        public void TeardownRunner()
        {
            runner.Dispose();
        }
    }
}
