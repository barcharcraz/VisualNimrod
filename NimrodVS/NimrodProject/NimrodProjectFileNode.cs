using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project.Automation;
using Microsoft.VisualStudio.Project;
namespace Company.NimrodVS.ManagedNimrodProject
{
    public class NimrodProjectFileNode : FileNode
    {
        private OANimrodProjectFileItem automationObject;
        internal NimrodProjectFileNode(ProjectNode root, ProjectElement e)
            : base(root, e)
        {

        }
        public override object GetAutomationObject()
        {
            if (automationObject == null)
            {
                automationObject = new OANimrodProjectFileItem(this.ProjectMgr.GetAutomationObject() as OAProject, this);
            }
            return automationObject;
        }
        private object CreateServices(Type serviceType)
        {
            object service = null;
            if (typeof(EnvDTE.ProjectItem) == serviceType)
            {
                service = GetAutomationObject();
            }
            return service;
        }
        internal OleServiceProvider.ServiceCreatorCallback ServiceCreator
        {
            get { return new OleServiceProvider.ServiceCreatorCallback(this.CreateServices); }
        }
    }
}
