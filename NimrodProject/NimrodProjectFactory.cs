using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
namespace NimrodProject
{
    public class NimrodProjectFactory : IVsProjectFactory
    {
        private ServiceProvider _serviceProvider;
        protected ServiceProvider ServiceProvider { get { return _serviceProvider; } }
        protected Package Package { get; private set; }
        public int CanCreateProject(string pszFilename, uint grfCreateFlags, out int pfCanCreate)
        {
            if (string.IsNullOrEmpty(pszFilename))
            {
                pfCanCreate = 0;
                return VSConstants.S_OK;
            } 
            else if (PackageUtilities.ContainsInvalidFileNameChars(pszFilename))
            {
                pfCanCreate = 0;
                //even though we can not create the file the method
                //still succeded, so we still return S_OK
                return VSConstants.S_OK;
            }
            else
            {
                pfCanCreate = 1;
                return VSConstants.S_OK;
            }
        }

        public int Close()
        {
            this.Dispose(true);
            return 0;
        }

        public int CreateProject(string pszFilename, string pszLocation, string pszName, uint grfCreateFlags, ref Guid iidProject, out IntPtr ppvProject, out int pfCanceled)
        {
            throw new NotImplementedException();
            NimrodProjectNode project = new NimrodProjectNode(this.Package);
        }

        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            _serviceProvider = new ServiceProvider(psp);
            return 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._serviceProvider != null)
            {
                this._serviceProvider.Dispose();
                this._serviceProvider = null;
            }
        }
    }
}
