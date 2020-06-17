using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;
using System.Configuration;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System.Collections.Generic;
using OpenQA.Selenium;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;

namespace Xrm.CI.Framework.Sample.UIAutomation
{
    [TestClass]
    public class CreateContactTest
    {
        public TestContext TestContext { get; set; }

        private readonly SecureString _username = CreateContactTest.GetConfigValue("CrmUsername").ToSecureString();
        private readonly SecureString _password = CreateContactTest.GetConfigValue("CrmPassword").ToSecureString();
        private readonly Uri _xrmUri = new Uri(CreateContactTest.GetConfigValue("CrmUrl"));

        private static string GetConfigValue(string name)
        {
            string value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(value))
                value = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrEmpty(value))
                throw new NotFoundException(name);
            return value;
        }

        private readonly BrowserOptions _options = new BrowserOptions
        {
            BrowserType = BrowserType.Chrome,
            PrivateMode = true,
            FireEvents = true
        };

        [TestMethod]
        public void CreateNewContact()
        {
            var client = new WebClient(_options);
            try
            {
                using (var xrmApp = new XrmApp(client))
                {
                    xrmApp.OnlineLogin.Login(_xrmUri, _username, _password);

                    TestContext.WriteLine("Login Successful");

                    xrmApp.Navigation.OpenApp("CRM Hub");

                    xrmApp.ThinkTime(500);
                    xrmApp.Navigation.OpenSubArea("Sales", "Contacts");

                    //xrmApp.ThinkTime(2000);
                    //xrmApp.Grid.SwitchView("Active Contacts");

                    xrmApp.ThinkTime(1000);
                    xrmApp.CommandBar.ClickCommand("New");

                    TestContext.WriteLine("Entering Contact");

                    xrmApp.ThinkTime(2000);

                    xrmApp.Entity.SetValue("firstname", "Wael");
                    xrmApp.Entity.SetValue("lastname", DateTime.Now.ToString());
                    xrmApp.Entity.SetValue("emailaddress1", new Random().Next(100000, 9900000).ToString() + "@contoso.com");
                    xrmApp.Entity.SetValue("mobilephone", new Random().Next(100000, 9900000).ToString());

                    xrmApp.Entity.SelectTab("Details");

                    xrmApp.Entity.SetValue("birthdate", DateTime.Parse("11/1/1983"));
                    xrmApp.Entity.SetValue(new OptionSet { Name = "preferredcontactmethodcode", Value = "Email" });

                    xrmApp.CommandBar.ClickCommand("Save");
                    xrmApp.ThinkTime(2000);

                    TestContext.WriteLine("Contact Saved");

                    string screenShot = string.Format("{0}\\CreateNewContact.jpeg", TestContext.TestResultsDirectory);

                    client.Browser.TakeWindowScreenShot(screenShot, ScreenshotImageFormat.Jpeg);

                    TestContext.WriteLine($"Screenshot saved to: {screenShot}");
                    TestContext.AddResultFile(screenShot);
                }
            }
            finally
            {
                if (client.Browser != null)
                {
                    client.Browser.Dispose();
                }
            }
        }
    }
}
