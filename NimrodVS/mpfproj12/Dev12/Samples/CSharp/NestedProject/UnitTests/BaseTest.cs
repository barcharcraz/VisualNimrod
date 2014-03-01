/********************************************************************************************

Copyright (c) Microsoft Corporation 
All rights reserved. 

Microsoft Public License: 

This license governs use of the accompanying software. If you use the software, you 
accept this license. If you do not accept the license, do not use the software. 

1. Definitions 
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the 
same meaning here as under U.S. copyright law. 
A "contribution" is the original software, or any additions or changes to the software. 
A "contributor" is any person that distributes its contribution under this license. 
"Licensed patents" are a contributor's patent claims that read directly on its contribution. 

2. Grant of Rights 
(A) Copyright Grant- Subject to the terms of this license, including the license conditions 
and limitations in section 3, each contributor grants you a non-exclusive, worldwide, 
royalty-free copyright license to reproduce its contribution, prepare derivative works of 
its contribution, and distribute its contribution or any derivative works that you create. 
(B) Patent Grant- Subject to the terms of this license, including the license conditions 
and limitations in section 3, each contributor grants you a non-exclusive, worldwide, 
royalty-free license under its licensed patents to make, have made, use, sell, offer for 
sale, import, and/or otherwise dispose of its contribution in the software or derivative 
works of the contribution in the software. 

3. Conditions and Limitations 
(A) No Trademark License- This license does not grant you rights to use any contributors' 
name, logo, or trademarks. 
(B) If you bring a patent claim against any contributor over patents that you claim are 
infringed by the software, your patent license from such contributor to the software ends 
automatically. 
(C) If you distribute any portion of the software, you must retain all copyright, patent, 
trademark, and attribution notices that are present in the software. 
(D) If you distribute any portion of the software in source code form, you may do so only 
under this license by including a complete copy of this license with your distribution. 
If you distribute any portion of the software in compiled or object code form, you may only 
do so under a license that complies with this license. 
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give 
no express warranties, guarantees or conditions. You may have additional consumer rights 
under your local laws which this license cannot change. To the extent permitted under your 
local laws, the contributors exclude the implied warranties of merchantability, fitness for 
a particular purpose and non-infringement.

********************************************************************************************/

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.UnitTestLibrary;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using MSBuild = Microsoft.Build.BuildEngine;
using OleServiceProvider = Microsoft.VsSDK.UnitTestLibrary.OleServiceProvider;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject.UnitTests
{
    /// <summary>
    /// NestedProjectFactory fake object creates NestedProjectNode fake object
    /// </summary>
    public class NestedProjectFactoryFake : NestedProjectFactory
    {
        public NestedProjectFactoryFake(NestedProjectPackage package)
            : base(package)
        {
        }

        protected override ProjectNode CreateProject()
        {
            NesteProjectNodeFake project = new NesteProjectNodeFake();
            project.SetSite((IOleServiceProvider)((IServiceProvider)this.Package).GetService(typeof(IOleServiceProvider)));
            return project;
        }
    }

    /// <summary>
    /// This is a fake object for NestedPRojectNode so that we can skip certain method calls,
    /// e.g. ProcessRefrences involves a build and we would like to skip that
    /// </summary>
    public class NesteProjectNodeFake : NestedProjectNode
    {
        protected internal override void ProcessReferences()
        {
            return;
        }
    }

    [TestClass]
    public abstract class BaseTest
    {
        protected Microsoft.VsSDK.UnitTestLibrary.OleServiceProvider serviceProvider;
        protected TestContext testContextInstance;
        protected static string fullPathToClassTemplateFile = @"TemplateClass.cs";
        protected static string fullPathToProjectFile = @"SampleProject.csproj";
        protected static string fullPathToTargetFile = @"SampleClass.cs";

        protected GeneralPropertyPage generalPropertyPage;

        protected NestedProjectPackage projectPackage;
        protected NestedProjectFactoryFake projectFactory;
        protected NesteProjectNodeFake projectNode;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize()]
        public virtual void Initialize()
        {
            this.MockServices();
            this.LoadProject();
            UIThread.IsUnitTest = true;
        }

        /// <summary>
        /// Runs after the test has run and to free resources obtained 
        /// by all the tests in the test class.
        /// </summary>
        // [TestCleanup()]
        public void Cleanup()
        {
            ((IVsPackage)projectPackage).SetSite(null);
            serviceProvider.Dispose();

            generalPropertyPage = null;
        }

        protected virtual void SetMsbuildEngine(ProjectFactory factory)
        {
            ProjectCollection.GlobalProjectCollection.UnloadAllProjects();

            FieldInfo buildEngine = typeof(ProjectFactory).GetField("buildEngine", BindingFlags.Instance | BindingFlags.NonPublic);
            buildEngine.SetValue(factory, ProjectCollection.GlobalProjectCollection);

            Microsoft.Build.Evaluation.Project msbuildproject = ProjectCollection.GlobalProjectCollection.LoadProject(fullPathToProjectFile);
            FieldInfo buildProject = typeof(ProjectFactory).GetField("buildProject", BindingFlags.Instance | BindingFlags.NonPublic);
            buildProject.SetValue(factory, msbuildproject);
        }

        protected virtual void MockServices()
        {
            serviceProvider = Microsoft.VsSDK.UnitTestLibrary.OleServiceProvider.CreateOleServiceProviderWithBasicServices();

            // Add solution Support
            BaseMock solution = MockServicesProvider.GetSolutionFactoryInstance();
            serviceProvider.AddService(typeof(IVsSolution), solution, false);

            //Add site support for ILocalRegistry
            BaseMock localRegistry = MockServicesProvider.GetLocalRegistryInstance();
            serviceProvider.AddService(typeof(SLocalRegistry), (ILocalRegistry)localRegistry, false);

            // Add site support for UI Shell
            BaseMock uiShell = MockServicesProvider.GetUiShellInstance0();
            serviceProvider.AddService(typeof(SVsUIShell), uiShell, false);
            serviceProvider.AddService(typeof(SVsUIShellOpenDocument), (IVsUIShellOpenDocument)uiShell, false);

            // Add site support for RegisterProjectTypes
            BaseMock mock = MockServicesProvider.GetRegisterProjectInstance();
            serviceProvider.AddService(typeof(SVsRegisterProjectTypes), mock, false);

            // Add site support for VsShell
            BaseMock vsShell = MockServicesProvider.GetVsShellInstance0();
            serviceProvider.AddService(typeof(SVsShell), vsShell, false);

            // Add site support for SolutionBuildManager service
            BaseMock solutionBuildManager = MockServicesProvider.GetSolutionBuildManagerInstance0();
            serviceProvider.AddService(typeof(SVsSolutionBuildManager), solutionBuildManager, false);


            // SVsFileChangeEx support
            BaseMock fileChangeEx = MockServicesProvider.GetIVsFileChangeEx();
            serviceProvider.AddService(typeof(SVsFileChangeEx), fileChangeEx, false);
        }
        
        protected virtual void LoadProject()
        {
            generalPropertyPage = new GeneralPropertyPage();

            // Prepare the package
            projectPackage = new NestedProjectPackage();
            ((IVsPackage)projectPackage).SetSite(serviceProvider);

            // prepare the factory
            projectFactory = new NestedProjectFactoryFake(projectPackage);
            this.SetMsbuildEngine(projectFactory);

            //Create the project object using the projectfactory and load the project
            int canCreate;
            if (VSConstants.S_OK == ((IVsProjectFactory)projectFactory).CanCreateProject(fullPathToProjectFile, 2, out canCreate))
            {
                MethodInfo preCreateForOuter = typeof(NestedProjectFactory).GetMethod("PreCreateForOuter", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(preCreateForOuter, "failed to get the PreCreateForOuter method info object from NestedProjectFactory type");
                projectNode = (NesteProjectNodeFake)preCreateForOuter.Invoke(projectFactory, new object[] { IntPtr.Zero });
                Assert.IsNotNull(projectNode, "Failed to create the projectnode object");
                Guid iidProject = new Guid();
                int pfCanceled;
                projectNode.Load(fullPathToProjectFile, "", "", 2, ref iidProject, out pfCanceled);
            }

        }
    }
}
