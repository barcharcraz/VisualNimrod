using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.Versioning;
using System.ComponentModel;
using System.IO;
using Company.NimrodVS.Attributes;

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

        public NimrodGeneralPropertyPage()
        {
            this.Name = NimrodResources.GetString(NimrodResources.GeneralCaption);
        }
        [NimCategory(NimrodResources.AssemblyName)]
        [NimLocDisplayName(NimrodResources.AssemblyName)]
        [NimDescription(NimrodResources.AssemblyNameDescription)]
        public string AssemblyName
        {
            get { return this.assemblyName; }
            set { this.assemblyName = value; this.IsDirty = true; }
        }
        [NimCategory(NimrodResources.Application)]
        [NimLocDisplayName(NimrodResources.OutputType)]
        [NimDescription(NimrodResources.OutputTypeDescription)]
        public OutputType OutputType
        {
            get { return this.outputType; }
            set { this.outputType = value; this.IsDirty = true; }
        }

        [NimCategory(NimrodResources.Application)]
        [NimLocDisplayName(NimrodResources.DefaultNamespace)]
        [NimDescription(NimrodResources.DefaultNamespaceDescription)]
        public string DefaultNamespace
        {
            get { return this.defaultNamespace; }
            set { this.defaultNamespace = value; this.IsDirty = true; }
        }

        [NimCategory(NimrodResources.Application)]
        [NimLocDisplayName(NimrodResources.StartupObject)]
        [NimDescription(NimrodResources.StartupObjectDescription)]
        public string StartupObject
        {
            get { return this.startupObject; }
            set { this.startupObject = value; this.IsDirty = true; }
        }

        [NimCategory(NimrodResources.Application)]
        [NimLocDisplayName(NimrodResources.ApplicationIcon)]
        [NimDescription(NimrodResources.ApplicationIconDescription)]
        public string ApplicationIcon
        {
            get { return this.applicationIcon; }
            set { this.applicationIcon = value; this.IsDirty = true; }
        }
        [NimCategory(NimrodResources.Project)]
        [NimLocDisplayName(NimrodResources.ProjectFile)]
        [NimDescription(NimrodResources.ProjectFileDescription)]
        public string ProjectFile
        {
            get { return Path.GetFileName(this.ProjectMgr.ProjectFile); }
        }
        [NimCategory(NimrodResources.Project)]
        [NimLocDisplayName(NimrodResources.ProjectFolder)]
        [NimDescription(NimrodResources.ProjectFolderDescription)]
        public string ProjectFolder
        {
            get { return Path.GetDirectoryName(this.ProjectMgr.ProjectFolder); }
        }
        [NimCategory(NimrodResources.Project)]
        [NimLocDisplayName(NimrodResources.OutputFile)]
        [NimDescription(NimrodResources.OutputFileDescription)]
        public string OutputFile
        {
            get
            {
                switch (this.outputType)
                {
                    case OutputType.Exe:
                    case OutputType.WinExe:
                        return this.assemblyName + ".exe";
                    case OutputType.Library:
                        return this.assemblyName + ".dll";
                    default:
                        return this.assemblyName;
                }
            }
        }

        public override string GetClassName()
        {
            return this.GetType().FullName;
        }
        protected override void BindProperties()
        {
            if (this.ProjectMgr == null)
            {
                return;
            }
            this.assemblyName = this.ProjectMgr.GetProjectProperty("AssemblyName", true);
            string outputType = this.ProjectMgr.GetProjectProperty("OutputType", false);
            if (outputType != null && outputType.Length > 0)
            {
                this.outputType = (OutputType)Enum.Parse(typeof(OutputType), outputType);
            }
            this.defaultNamespace = this.ProjectMgr.GetProjectProperty("RootNamespace", false);
            this.startupObject = this.ProjectMgr.GetProjectProperty("StartupObject", false);
            this.applicationIcon = this.ProjectMgr.GetProjectProperty("ApplicationIcon", false);
            
        }
        protected override int ApplyChanges()
        {
            if (this.ProjectMgr == null)
            {
                return VSConstants.E_INVALIDARG;
            }
            IVsPropertyPageFrame propertyPageFrame = (IVsPropertyPageFrame)this.ProjectMgr.Site.GetService((typeof(SVsPropertyPageFrame)));
            this.ProjectMgr.SetProjectProperty("AssemblyName", this.assemblyName);
            this.ProjectMgr.SetProjectProperty("OutputType", this.outputType.ToString());
            this.ProjectMgr.SetProjectProperty("RootNamespace", this.defaultNamespace);
            this.ProjectMgr.SetProjectProperty("StartupObject", this.startupObject);
            this.ProjectMgr.SetProjectProperty("ApplicationIcon", this.applicationIcon);
            this.IsDirty = false;
            return VSConstants.S_OK;

        }

    }
}
