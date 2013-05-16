using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuCore;
using FubuCore.Dates;
using FubuCore.Reflection;
using FubuMVC.Core.Urls;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Assertions;
using StoryTeller.Engine;
using TodoSite;

namespace TodoSite.StoryTeller.Fixtures
{

    public class LoginScreenFixture : ScreenFixture
    {
        public LoginScreenFixture()
        {
            Title = "Login Screen";
        }

        [FormatAs("Recycle the browser")]
        public void RecycleTheBrowser()
        {
            Retrieve<IApplicationUnderTest>().Browser.Recycle();
        }

        [FormatAs("Go to the login screen")]
        public void OpenLoginScreen()
        {
            Navigation.NavigateTo(new UserInputModel());
        }

        [FormatAs("Logout")]
        public void Logout()
        {
            IApplicationUnderTest app = Retrieve<IApplicationUnderTest>();

            var ha = app.GetHashCode();

            Navigation.NavigateTo<UserLoginController>(x => x.Home());
            return;
            //Navigation.NavigateTo(new UserInputModel());
            //Retrieve<IApplicationUnderTest>().RootUrl
            //Console.Write("h1");
            //IApplicationUnderTest app = Retrieve<IApplicationUnderTest>();
            IUrlRegistry urls = Retrieve<IApplicationUnderTest>().Urls;
            string str = "GET";
            object model = new UserInputModel();
            string categoryOrHttpMethod = str;
            //var url = urls.UrlFor(model, categoryOrHttpMethod);
            //Console.WriteLine("url: " + url);
            //Navigation.NavigateToUrl(urls.UrlFor<UserLoginController>(x => x.get_Home(new UserInputModel()), "GET"));
            Navigation.NavigateToUrl(urls.UrlFor(model, categoryOrHttpMethod));
            //Navigation.NavigateTo(new UserInputModel());
        }
    }
}
