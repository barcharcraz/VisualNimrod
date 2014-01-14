using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
namespace Company.NimrodVS.NimrodProject
{
    [ComVisible(true)]
    public class NimrodGeneralPropertyPage : SettingsPage
    {
        private string assemblyName;
        private OutputType outputType;
        private string defaultNamespace;
        private string startupObject;
        private string applicationIcon;
        private FrameworkName targetFrameworkMoniker;

        public NimrodGeneralPropertyPage()
        {
            this.Name = NimrodResources.GetString(NimrodResources.GeneralCaption);
        }
    }
}
