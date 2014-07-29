using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Globalization;
namespace Company.NimrodVS
{
    public class NimrodResources
    {
        internal const string Application = "Application";
        internal const string ApplicationCaption = "ApplicationCaption";
        internal const string GeneralCaption = "GeneralCaption";
        internal const string AssemblyName = "AssemblyName";
        internal const string AssemblyNameDescription = "AssemblyNameDescription";
        internal const string OutputType = "OutputType";
        internal const string OutputTypeDescription = "OutputTypeDescription";
        internal const string DefaultNamespace = "DefaultNamespace";
        internal const string DefaultNamespaceDescription = "DefaultNamespaceDescription";
        internal const string StartupObject = "StartupObject";
        internal const string StartupObjectDescription = "StartupObjectDescription";
        internal const string ApplicationIcon = "ApplicationIcon";
        internal const string ApplicationIconDescription = "ApplicationIconDescription";
        internal const string Project = "Project";
        internal const string ProjectFile = "ProjectFile";
        internal const string ProjectFileDescription = "ProjectFileDescription";
        internal const string ProjectFolder = "ProjectFolder";
        internal const string ProjectFolderDescription = "ProjectFolderDescription";
        internal const string OutputFile = "OutputFile";
        internal const string OutputFileDescription = "OutputFileDescription";
        internal const string TargetFrameworkMoniker = "TargetFrameworkMoniker";
        internal const string TargetFrameworkMonikerDescription = "TargetFrameworkMonikerDescription";
        internal const string NestedProjectFileAssemblyFilter = "NestedProjectFileAssemblyFilter";

        internal const string CCompiler = "CCompiler";
        internal const string CCompilerDescription = "Target C Compiler";

        private static NimrodResources loader;
        private ResourceManager resourceManager;
        private static Object internalSyncObjectInstance;

        internal NimrodResources() {
            resourceManager = new ResourceManager("Company.NimrodVS.Resources", Assembly.GetExecutingAssembly());
        }
        private static Object InternalSyncObject
        {
            get
            {
                if (internalSyncObjectInstance == null)
                {
                    Object o = new Object();
                    Interlocked.CompareExchange(ref internalSyncObjectInstance, o, null);
                }
                return internalSyncObjectInstance;
            }
        }
        public static CultureInfo Culture
        {
            get { return null; }
        }

        public static ResourceManager ResourceManager
        {
            get { return GetLoader().resourceManager; }
        }
        public static string GetString(string name, params object[] args)
        {
            NimrodResources resInstance = GetLoader();
            if (resInstance == null)
            {
                return null;
            }
            string res = resInstance.resourceManager.GetString(name, NimrodResources.Culture);
            if (args != null && args.Length > 0)
            {
                return String.Format(CultureInfo.CurrentCulture, res, args);
            }
            else
            {
                return res;
            }
        }
        public static string GetString(string name)
        {
            NimrodResources resInstance = GetLoader();
            if (resInstance == null)
            {
                return null;
            }
            return resInstance.resourceManager.GetString(name, NimrodResources.Culture);
        }
        public static object GetObject(string name)
        {
            NimrodResources resInstance = GetLoader();
            if (resInstance == null)
            {
                return null;
            }
            return resInstance.resourceManager.GetObject(name, NimrodResources.Culture);
        }
        private static NimrodResources GetLoader()
        {
            if (loader == null)
            {
                lock (InternalSyncObject)
                {
                    if (loader == null)
                    {
                        loader = new NimrodResources();
                    }
                }
            }
            return loader;
        }
    }
}
