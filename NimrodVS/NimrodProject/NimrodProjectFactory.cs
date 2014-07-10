using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
namespace Company.NimrodVS.ManagedNimrodProject
{
    public class NimrodProjectFactory : ProjectFactory
    {
        private NimrodVSPackage package;
        public NimrodProjectFactory(NimrodVSPackage package)
            : base(package)
        {
            this.package = package;
            
        }

        protected override ProjectNode CreateProject()
        {
            NimrodProjectNode project = new NimrodProjectNode(this.package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)this.package).GetService(typeof(IOleServiceProvider)));
            return project;
        }
    }
}
