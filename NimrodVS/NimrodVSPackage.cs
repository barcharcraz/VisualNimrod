using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Project;
using NimrodSharp;

namespace Company.NimrodVS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\10.0")]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidNimrodVSPkgString)]
    [ProvideObject(typeof(ManagedNimrodProject.NimrodGeneralPropertyPage))]
    [ProvideObject(typeof(BuildPropertyPage))]
    [ProvideService(typeof(NimrodLanguageService), ServiceName="Nimrod Language Service")]
    [ProvideLanguageService(typeof(NimrodLanguageService), "Nimrod", 106, CodeSense=true, EnableFormatSelection = true,
        RequestStockColors=true)]
    [ProvideLanguageExtension(typeof(NimrodLanguageService), ".nim")]
    [ProvideProjectFactory(typeof(ManagedNimrodProject.NimrodProjectFactory), null, "Nimrod Projects (*.nimproj);*.nimproj", "nimproj", "nimproj", ".\\NullPath", LanguageVsTemplate = "NimrodProject", NewProjectRequireNewFolderVsTemplate=false)]
    //[ProvideProjectItem(typeof(ManagedNimrodProject.NimrodProjectFactory), "Nimrod Source", @"..\..\Templates\ProjectItems\NimrodProject", 500)]
    public sealed class NimrodVSPackage : ProjectPackage, IOleComponent
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public NimrodVSPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }


        private uint m_componentID;
        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
            var projFact = new ManagedNimrodProject.NimrodProjectFactory(this);
            this.RegisterProjectFactory(projFact);
            
            //proffer the service
            IServiceContainer serviceContainer = this as IServiceContainer;
            NimrodLanguageService langSvc = new NimrodLanguageService();
            langSvc.SetSite(this);
            
            serviceContainer.AddService(typeof(NimrodLanguageService),
                langSvc, true);
            //register a time to call our service during idle periods
            IOleComponentManager mgr = GetService(typeof(SOleComponentManager))
                                                    as IOleComponentManager;
            if (m_componentID == 0 && mgr != null)
            {
                OLECRINFO[] crinfo = new OLECRINFO[1];
                crinfo[0].cbSize = (uint)Marshal.SizeOf(typeof(OLECRINFO));
                crinfo[0].grfcrf = (uint)_OLECRF.olecrfNeedIdleTime |
                                   (uint)_OLECRF.olecrfNeedPeriodicIdleTime;
                crinfo[0].grfcadvf = (uint)_OLECADVF.olecadvfModal |
                                     (uint)_OLECADVF.olecadvfRedrawOff |
                                     (uint)_OLECADVF.olecadvfWarningsOff;
                crinfo[0].uIdleTimeInterval = 1000;
                int hr = mgr.FRegisterComponent(this, crinfo, out m_componentID);
                Marshal.ThrowExceptionForHR(hr);
            }
            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidNimrodVSCmdSet, (int)PkgCmdIDList.cmdidInsertSnippet);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID );
                mcs.AddCommand( menuItem );
            }
        }
        public override string ProductUserContext
        {
            get { return "Visual Nimrod"; }
        }
        protected override void Dispose(bool disposing)
        {
            if (m_componentID != 0)
            {
                IOleComponentManager mgr = GetService(typeof(SOleComponentManager))
                                            as IOleComponentManager;
                if (mgr != null)
                {
                    int hr = mgr.FRevokeComponent(m_componentID);
                    try
                    {
                        Marshal.ThrowExceptionForHR(hr);
                    }
                    catch (Exception e)
                    {
                        Debug.Fail("revoke component failed with HR " + e.HResult);
                    }
                }
            }
            base.Dispose(disposing);
        }
        #endregion
        #region IOleComponent Members
        public int FDoIdle(uint grfidlef)
        {
            bool bperiodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;
            LanguageService langsvc = GetService(typeof(NimrodLanguageService))
                                      as LanguageService;
            if (langsvc != null)
            {
                langsvc.OnIdle(bperiodic);
            }
            return 0;
        }
        public int FContinueMessageLoop(uint uReason, IntPtr pvData, MSG[] pMsgPeeked)
        {
            return 1;
        }
        public int FPreTranslateMessage(MSG[] pMsg)
        {
            return 0;
        }
        public int FQueryTerminate(int fPromptUser)
        {
            return 1;
        }
        public int FReserved1(uint dwReserved, uint message, IntPtr wparam, IntPtr lparam)
        {
            return 1;
        }
        public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved)
        {
            return IntPtr.Zero;
        }
        public void OnActivationChange(
            IOleComponent pic,
            int fSameComponent,
            OLECRINFO[] pcrinfo,
            int fHostIsActivateing,
            OLECHOSTINFO[] pchostinfo,
            uint dwReserved)
        {

        }
        public void OnAppActivate(int fActive, uint dwOtherThreadID)
        {

        }
        public void OnEnterState(uint uStateID, int fEnter)
        {

        }
        public void OnLoseActivation()
        {

        }
        public void Terminate()
        {

        }
        #endregion
        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                       0,
                       ref clsid,
                       "NimrodVS",
                       string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out result));
        }

    }
}
