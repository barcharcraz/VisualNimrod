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
using System.Runtime.Versioning;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.UnitTestLibrary;
using MSBuild = Microsoft.Build.BuildEngine;
using OleServiceProvider = Microsoft.VsSDK.UnitTestLibrary.OleServiceProvider;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject.UnitTests
{
    /// <summary>
    ///This is a test class for VisualStudio.Project.Samples.NestedProject.GeneralPropertyPage and is intended
    ///to contain all VisualStudio.Project.Samples.NestedProject.GeneralPropertyPage Unit Tests
    ///</summary>
    [TestClass()]
    public class GeneralPropertyPageTest : BaseTest
    {
        #region Fields

        private string testString;
        VisualStudio_Project_Samples_GeneralPropertyPageAccessor gppAccessor;

        #endregion Fields


        #region Initialization && Cleanup

        /// <summary>
        /// Runs before the test to allocate and configure resources needed 
        /// by all tests in the test class.
        /// </summary>
        [TestInitialize()]
        public void GeneralPropertyPageTestInitialize()
        {
            base.Initialize();
            testString = "This is a test string";

            // Initialize GeneralPropertyPage instance
            gppAccessor = new VisualStudio_Project_Samples_GeneralPropertyPageAccessor(generalPropertyPage);
        }

        [ClassInitialize]
        public static void TestClassInitialize(TestContext context)
        {
            fullPathToProjectFile = Path.Combine(context.TestDeploymentDir, fullPathToProjectFile);
        }

        #endregion Initialization && Cleanup


        #region Test methods
        /// <summary>
        /// The test for ApplicationIcon.
        /// AppIcon must be internally assigned and isDirty flag switched on.
        ///</summary>
        [TestMethod()]
        public void ApplicationIconTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            target.ApplicationIcon = testString;

            Assert.AreEqual(testString, gppAccessor.applicationIcon, target.ApplicationIcon,
                "ApplicationIcon value was not initialized by expected value.");
            Assert.IsTrue((VSConstants.S_OK == target.IsPageDirty()),
                "IsDirty status was unexpected after changing of the property of the tested object.");
        }
        /// <summary>
        /// The test for ApplyChanges() in scenario when ProjectMgr is uninitialized.
        ///</summary>
        [TestMethod()]
        public void ApplyChangesNullableProjectMgrTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            // sets indirectly projectMgr to null
            target.SetObjects(0, null);
            int actual = gppAccessor.ApplyChanges();
            Assert.IsNull(target.ProjectMgr, "ProjectMgr instance was not initialized to null as it expected.");
            Assert.AreEqual(VSConstants.E_INVALIDARG, actual, "Method ApplyChanges() was returned unexpected value in case of uninitialized project instance.");
        }
        /// <summary>
        /// The test for AssemblyName property.
        ///</summary>
        [TestMethod()]
        public void AssemblyNameTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            target.AssemblyName = testString;
            Assert.AreEqual(testString, gppAccessor.assemblyName, target.ApplicationIcon,
                "AssemblyName property value was not initialized by expected value.");
            Assert.IsTrue((VSConstants.S_OK == target.IsPageDirty()), "IsDirty status was unexpected after changing of the property of the tested object.");
        }
        /// <summary>
        /// The test for GeneralPropertyPage default constructor.
        ///</summary>
        [TestMethod()]
        public void ConstructorTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            target.Name = testString;
            Assert.AreEqual(testString, target.Name, target.ApplicationIcon,
                "Name property value was not initialized by expected value in GeneralPropertyPage() constructor.");
        }
        /// <summary>
        /// The test for DefaultNamespace property.
        ///</summary>
        [TestMethod()]
        public void DefaultNamespaceTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            target.DefaultNamespace = testString;
            Assert.AreEqual(testString, target.DefaultNamespace, "DefaultNamespace property value was not initialized by expected value;");
            Assert.IsTrue((VSConstants.S_OK == target.IsPageDirty()), "IsDirty status was unexpected after changing of the property of the tested object.");
        }
        /// <summary>
        /// The test for GetClassName()  method.
        ///</summary>
        [TestMethod()]
        public void GetClassNameTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            string expectedClassName = "Microsoft.VisualStudio.Project.Samples.NestedProject.GeneralPropertyPage";
            string actualClassName = target.GetClassName();

            Assert.AreEqual(expectedClassName, actualClassName,
                "GetClassName() method was returned unexpected Type FullName value.");
        }
        /// <summary>
        /// The test for OutputFile in case of OutputType.Exe file type.
        ///</summary>
        [TestMethod()]
        public void OutputFileWithExeTypeTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            gppAccessor.outputType = OutputType.Exe;
            string expectedValue = target.AssemblyName + ".exe";

            Assert.AreEqual(expectedValue, target.OutputFile,
                "OutputFile name was initialized by unexpected value for EXE OutputType.");
        }
        /// <summary>
        /// The test for OutputFile property in case of using of OutputType.WinExe file type.
        ///</summary>
        [TestMethod()]
        public void OutputFileWithWinExeTypeTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            gppAccessor.outputType = OutputType.WinExe;
            string expectedValue = target.AssemblyName + ".exe";

            Assert.AreEqual(expectedValue, target.OutputFile,
                "OutputFile name was initialized by unexpected value for WINEXE OutputType.");
        }
        /// <summary>
        /// The test for OutputFile in case of using of OutputType.Library file type.
        ///</summary>
        [TestMethod()]
        public void OutputFileWithLibraryTypeTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            gppAccessor.outputType = OutputType.Library;
            string expectedValue = target.AssemblyName + ".dll";

            Assert.AreEqual(expectedValue, target.OutputFile,
                "OutputFile name was initialized by unexpected value for Library OutputType.");
        }
        /// <summary>
        /// The test for OutputType property.
        ///</summary>
        [TestMethod()]
        public void OutputTypeTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            OutputType expectedOutType = OutputType.Library;
            target.OutputType = expectedOutType;

            Assert.AreEqual(expectedOutType, target.OutputType,
                "OutputType property value was initialized by unexpected value.");
            Assert.IsTrue((VSConstants.S_OK == target.IsPageDirty()),
                "IsDirty status was unexpected after changing of the property of the tested object.");
        }
        /// <summary>
        /// The test for StartupObject property.
        ///</summary>
        [TestMethod()]
        public void StartupObjectTest()
        {
            GeneralPropertyPage target = generalPropertyPage;
            target.StartupObject = testString;
            Assert.AreEqual(testString, gppAccessor.startupObject, target.StartupObject,
                "StartupObject property value was not initialized by expected value.");
            Assert.IsTrue((VSConstants.S_OK == target.IsPageDirty()),
                "IsDirty status was unexpected after changing of the property of the tested object.");
        }
        /// <summary>
        /// The test for TargetPlatform property.
        ///</summary>
        [TestMethod()]
        public void TargetFrameworkMonikerTest()
        {
            var fx4 = new FrameworkName(".NETFramework", new Version(4, 1));
            generalPropertyPage.TargetFrameworkMoniker = fx4;

            FrameworkName expected = fx4;
            FrameworkName actual = generalPropertyPage.TargetFrameworkMoniker;

            Assert.AreEqual(expected, actual,
                "TargetFrameworkMoniker value was not initialized to expected value.");

            Assert.IsTrue((VSConstants.S_OK == generalPropertyPage.IsPageDirty()),
                "IsDirty status was unexpected after changing of the property of the tested object.");
        }

        /// <summary>
        /// The test for BindProperties() method.
        ///</summary>
        [TestMethod()]
        public void BindPropertiesTest()
        {
            PrepareProjectConfig();
            gppAccessor.defaultNamespace = null;
            gppAccessor.startupObject = null;
            gppAccessor.applicationIcon = null;
            gppAccessor.assemblyName = null;
            gppAccessor.targetFrameworkMoniker = null;

            // NOTE: For the best test result we must tests all shown below fields:
            // For this we must Load project with specified property values.
            //gppAccessor.targetPlatform
            //gppAccessor.targetPlatformLocation
            //gppAccessor.defaultNamespace
            //gppAccessor.startupObject
            //gppAccessor.applicationIcon

            gppAccessor.BindProperties();
            Assert.IsNotNull(gppAccessor.assemblyName, "The AssemblyName was not properly initialized.");
        }
        /// <summary>
        /// The test for BindProperties() method in scenario when ProjectMgr is not initialized.
        ///</summary>

        [TestMethod()]
        public void BindPropertiesWithNullableProjectMgrTest()
        {
            gppAccessor.BindProperties();

            Assert.IsNull(gppAccessor.assemblyName,
                "The AssemblyName was initialized in scenario when ProjectMgr is invalid.");
        }
        /// <summary>
        /// The test for ProjectFile property.
        ///</summary>
        [TestMethod()]
        public void ProjectFileTest()
        {
            PrepareProjectConfig();
            GeneralPropertyPage target = generalPropertyPage;

            // Project File Name must be equivalent with name of the currently loaded project
            Assert.AreEqual(Path.GetFileName(fullPathToProjectFile), target.ProjectFile,
                "ProjectFile property value was initialized by unexpected path value.");
        }
        /// <summary>
        ///The test for ProjectFolder property.
        ///</summary>
        [TestMethod()]
        public void ProjectFolderTest()
        {
            PrepareProjectConfig();
            GeneralPropertyPage target = generalPropertyPage;

            string expectedProjectFolderPath = Path.GetDirectoryName(fullPathToProjectFile);
            expectedProjectFolderPath = Path.GetDirectoryName(expectedProjectFolderPath);

            // Project Folder path must be equivalent with path of the currently loaded project
            Assert.AreEqual(expectedProjectFolderPath, target.ProjectFolder,
                "ProjectFolder property value was initialized by unexpected path value.");

        }
        /// <summary>
        /// The test for ApplyChanges() in scenario when ProjectMgr is initialized.
        ///</summary>
        [TestMethod()]
        public void ApplyChangesTest()
        {
            PrepareProjectConfig();
            int actual = gppAccessor.ApplyChanges();

            Assert.AreEqual(VSConstants.S_OK, actual,
                "Method ApplyChanges() was returned unexpected value in case of initialized project instance.");
        }
        #endregion Completed test methods

        #region Service functions
        /// <summary>
        /// Initialize ProjectConfig and internal projectMgr objects.
        /// </summary>
        /// <remarks>Service function. Before calling this function projectNode must be 
        /// initialized by valid project data.</remarks>
        protected void PrepareProjectConfig()
        {
            object[] ppUnk = new object[2];
            ProjectConfig pjc = new ProjectConfig(projectNode, "manualSetConfigArgument");
            ppUnk[0] = pjc;
            generalPropertyPage.SetObjects(1, ppUnk);
        }
        #endregion Service functions
    }
}

