using System;
using System.Configuration;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace Xrm.CI.Framework.Sample.IntegrationTests
{
    [TestClass]
    public class TestCreateContact
    {
        [TestMethod]
        public void CreateNewContact()
        {
            //Setup
            string crmCon = TestCreateContact.GetConfigValue("CrmConnection");

            using (CrmServiceClient svc = new CrmServiceClient(crmCon))
            {
                // Act
                Entity c = new Entity("contact");

                c["firstname"] = "Test";
                c["lastname"] = DateTime.Now.Date.ToShortDateString();

                c.Id = svc.Create(c);

                //Validate
                Thread.Sleep(5000);

                QueryByAttribute query = new QueryByAttribute("phonecall");
                query.Attributes.Add("regardingobjectid");
                query.Values.Add(c.Id);

                EntityCollection calls = svc.RetrieveMultiple(query);

                Assert.AreEqual(calls.Entities.Count, 1);

                //Clean up
                svc.Delete(c.LogicalName, c.Id);
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
