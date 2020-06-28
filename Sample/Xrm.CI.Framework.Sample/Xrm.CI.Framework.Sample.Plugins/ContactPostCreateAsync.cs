using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xrm.CI.Framework.Sample.Plugins
{
    public class ContactPostCreateAsync : IPlugin
    {
        public ContactPostCreateAsync()
        {
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference.
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                var target = context.InputParameters["Target"] as Entity;

                Entity call = new Entity("phonecall");
                call["subject"] = "New phonecall";
                call["regardingobjectid"] = target.ToEntityReference();

                Guid callId = service.Create(call);
            }
        }
    }
}
