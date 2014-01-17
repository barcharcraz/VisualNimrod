using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSLangProj;
using EnvDTE;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;
namespace Company.NimrodVS.NimrodProject
{
    public class NimrodProjectNode : ProjectNode
    {
        internal const string ProjectTypeName = "NimrodProject";
        private NimrodVSPackage package;
        private VSLangProj.VSProject vsProject;

        public NimrodProjectNode(NimrodVSPackage package)
        {
            this.package = package;
            this.CanProjectDeleteItems = true;
        }
        protected internal VSLangProj.VSProject VSProject
        {
            get
            {
                if (vsProject == null)
                {
                    vsProject = new OAVSProject(this);
                }
                return vsProject;
            }
        }
        #region Overrides
        public override Guid ProjectGuid
        {
            get { return typeof(NimrodProjectFactory).GUID; }
        }
        public override string ProjectType
        {
            get { return ProjectTypeName; }
        }
        public override object GetAutomationObject()
        {
            return new OANimrodProject(this);
        }
        public override FileNode CreateFileNode(ProjectElement item)
        {
            NimrodProjectFileNode node = new NimrodProjectFileNode(this, item);
            node.OleServiceProvider.AddService(typeof(EnvDTE.Project), new OleServiceProvider.ServiceCreatorCallback(this.CreateServices), false);
            node.OleServiceProvider.AddService(typeof(ProjectItem), node.ServiceCreator, false);
            node.OleServiceProvider.AddService(typeof(VSProject), new OleServiceProvider.ServiceCreatorCallback(this.CreateServices), false);
            return node;
        }
        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(NimrodGeneralPropertyPage).GUID;
            return result;
        }
        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(NimrodGeneralPropertyPage).GUID;
            return result;
        }
        private object CreateServices(Type serviceType)
        {
            object service = null;
            if (typeof(VSLangProj.VSProject) == serviceType)
            {
                service = this.vsProject;
            }
            else if (typeof(EnvDTE.Project) == serviceType)
            {
                service = this.GetAutomationObject();
            }
            return service;
        }
        #endregion
    }
}
