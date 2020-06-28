using System;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System.Linq;
using Xrm.CI.Framework.Sample.Plugins;

namespace Xrm.CI.Framework.Sample.UnitTests
{
    [TestClass]
    public class TestContactPostCreateAsync
    {
        [TestMethod]
        public void CreatePhonecall()
        {
            //Setup
            var ctx = new XrmFakedContext();
            var contact = new Entity("contact") { Id = Guid.NewGuid() };
            ContactPostCreateAsync p = new ContactPostCreateAsync();

            //Act
            ctx.ExecutePluginWithTarget(p, contact);

            //Verify
            var calls = ctx.CreateQuery("phonecall").ToList<Entity>();
            Assert.AreEqual(calls.Count, 1);
            EntityReference regarding = (EntityReference)(calls[0]["regardingobjectid"]);
            Assert.AreEqual(regarding.Id, contact.Id);

            //Clean Up
        }
    }
}
