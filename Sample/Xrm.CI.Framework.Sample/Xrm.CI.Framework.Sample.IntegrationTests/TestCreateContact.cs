using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace Xrm.CI.Framework.Sample.IntegrationTests
{
    [TestClass]
    public class TestCreateContact
    {
        [TestMethod]
        public void CreateNewContact()
        {
            string crmCon = TestCreateContact.GetConfigValue("CrmConnection");

            using (CrmServiceClient svc = new CrmServiceClient(crmCon))
            {
                Entity c = new Entity("contact");

                c["firstname"] = "Test";
                c["lastname"] = DateTime.Now.Date.ToShortDateString();

                c.Id = svc.Create(c);
            }
        }
        private static string GetConfigValue(string name)
        {
            string value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(value))
                value = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            if (string.IsNullOrEmpty(value))
                throw new Exception(name);
            return value;
        }
    }
}
