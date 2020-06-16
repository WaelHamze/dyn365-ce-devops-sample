using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Uii.Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.PackageDeployment;
using Microsoft.Xrm.Tooling.PackageDeployment.CrmPackageExtentionBase;

namespace Xrm.CI.Framework.Sample.CRMPackage
{
    /// <summary>
    /// Import package starter frame. 
    /// </summary>
    [Export(typeof(IImportExtensions))]
    public class PackageTemplate : ImportExtension
    {
        /// <summary>
        /// Called When the package is initialized. 
        /// </summary>
        public override void InitializeCustomExtension()
        {
            if (RuntimeSettings != null)
            {
                PackageLog.Log(string.Format("Runtime Settings populated.  Count = {0}", RuntimeSettings.Count), TraceEventType.Verbose);
                foreach (var setting in RuntimeSettings)
                {
                    PackageLog.Log(string.Format("Key={0} | Value={1}", setting.Key, setting.Value.ToString()), TraceEventType.Verbose);
                }
            }
            else
            {
                PackageLog.Log("Runtime Settings not populated", TraceEventType.Verbose);
            }
        }

        /// <summary>
        /// Called Before Import Completes. 
        /// </summary>
        /// <returns></returns>
        public override bool BeforeImportStage()
        {
            return true; // do nothing here. 
        }

        /// <summary>
        /// Called for each UII record imported into the system
        /// This is UII Specific and is not generally used by Package Developers
        /// </summary>
        /// <param name="app">App Record</param>
        /// <returns></returns>
        public override ApplicationRecord BeforeApplicationRecordImport(ApplicationRecord app)
        {
            return app;  // do nothing here. 
        }

        /// <summary>
        /// Called during a solution upgrade while both solutions are present in the target CRM instance. 
        /// This function can be used to provide a means to do data transformation or upgrade while a solution update is occurring. 
        /// </summary>
        /// <param name="solutionName">Name of the solution</param>
        /// <param name="oldVersion">version number of the old solution</param>
        /// <param name="newVersion">Version number of the new solution</param>
        /// <param name="oldSolutionId">Solution ID of the old solution</param>
        /// <param name="newSolutionId">Solution ID of the new solution</param>
        public override void RunSolutionUpgradeMigrationStep(string solutionName, string oldVersion, string newVersion, Guid oldSolutionId, Guid newSolutionId)
        {

            base.RunSolutionUpgradeMigrationStep(solutionName, oldVersion, newVersion, oldSolutionId, newSolutionId);
        }

        /// <summary>
        /// Called after Import completes. 
        /// </summary>
        /// <returns></returns>
        public override bool AfterPrimaryImport()
        {
            PublishTheme();

            return true; // Do nothing here/ 
        }

        #region Methods

        private void PublishTheme()
        {
            string themeName = "xRMCISample";
            string logoName = "ud_/Images/logo.png";
            bool publish = false;

            QueryByAttribute qba = new QueryByAttribute("theme");
            qba.Attributes.Add("name");
            qba.Values.Add(themeName);
            qba.ColumnSet = new ColumnSet("themeid", "isdefaulttheme", "logoid");

            EntityCollection themes = base.CrmSvc.RetrieveMultiple(qba);

            if (themes.Entities.Count == 0)
            {
                throw new Exception("Theme not found");
            }

            Entity theme = themes.Entities[0];

            if (!(bool)theme.Attributes["isdefaulttheme"])
            {
                base.PackageLog.Log($"{themeName} is not published", TraceEventType.Information);

                publish = true;
            }

            Entity logo = GetLogo(logoName);

            if (!theme.Attributes.Contains("logoid"))
            {
                base.PackageLog.Log($"{logoName} is not set", TraceEventType.Information);

                theme["logoid"] = logo.ToEntityReference();
                publish = true;
            }
            else
            {
                if (logo.Id != ((EntityReference)theme["logoid"]).Id)
                {
                    base.PackageLog.Log($"{logoName} has changed", TraceEventType.Information);
                    publish = true;
                }
                else
                {
                    base.PackageLog.Log($"{logoName} is same", TraceEventType.Verbose);
                }
            }

            if (publish)
            {
                base.PackageLog.Log("Publishing Theme", TraceEventType.Verbose);

                PublishThemeRequest req = new PublishThemeRequest();
                req.Target = theme.ToEntityReference();
                base.CrmSvc.Execute(req);

                base.PackageLog.Log("Published Theme", TraceEventType.Information);
            }
        }

        private Entity GetLogo(string logoName)
        {
            QueryByAttribute qba = new QueryByAttribute("webresource");
            qba.Attributes.Add("name");
            qba.Values.Add(logoName);
            qba.ColumnSet = new ColumnSet();

            EntityCollection logos = base.CrmSvc.RetrieveMultiple(qba);

            if (logos.Entities.Count == 0)
            {
                throw new Exception("Logo not found");
            }

            Entity logo = logos.Entities[0];

            return logo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the Import Package to Use
        /// </summary>
        /// <param name="plural">if true, return plural version</param>
        /// <returns></returns>
        public override string GetNameOfImport(bool plural)
        {
            return "Package Short Name";
        }

        /// <summary>
        /// Folder Name for the Package data. 
        /// </summary>
        public override string GetImportPackageDataFolderName
        {
            get
            {
                // WARNING this value directly correlates to the folder name in the Solution Explorer where the ImportConfig.xml and sub content is located. 
                // Changing this name requires that you also change the correlating name in the Solution Explorer 
                return "PkgFolder";
            }
        }

        /// <summary>
        /// Description of the package, used in the package selection UI
        /// </summary>
        public override string GetImportPackageDescriptionText
        {
            get { return "Package Description"; }
        }

        /// <summary>
        /// Long name of the Import Package. 
        /// </summary>
        public override string GetLongNameOfImport
        {
            get { return "Package Long Name"; }
        }


        #endregion

    }
}
