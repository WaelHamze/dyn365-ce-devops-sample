using System;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Xrm.CI.Framework.Sample.Plugins;

namespace Xrm.CI.Framework.Sample.UnitTests
{
    [TestClass]
    public class TestContactPreOperation
    {
        [TestMethod]
        public void CreateNewContact()
        {
            var ctx = new XrmFakedContext();

            var contact = new Entity("contact") { Id = Guid.NewGuid() };

            ctx.ExecutePluginWithTarget<ContactPreOperation>(contact);
        }
    }
}
