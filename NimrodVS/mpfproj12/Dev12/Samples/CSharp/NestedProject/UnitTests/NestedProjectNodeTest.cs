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
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VsSDK.UnitTestLibrary;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using MSBuild = Microsoft.Build.BuildEngine;
using OleServiceProvider = Microsoft.VsSDK.UnitTestLibrary.OleServiceProvider;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject.UnitTests
{
    /// <summary>
    ///This is a test class for VisualStudio.Project.Samples.NestedProject.NestedProjectNode and is intended
    ///to contain all VisualStudio.Project.Samples.NestedProject.NestedProjectNode Unit Tests
    ///</summary>
    [TestClass()]
    public class NestedProjectNodeTest : BaseTest
    {
        [ClassInitialize]
        public static void TestClassInitialize(TestContext context)
        {
            fullPathToClassTemplateFile = Path.Combine(context.TestDeploymentDir, fullPathToClassTemplateFile);
            fullPathToProjectFile = Path.Combine(context.TestDeploymentDir, fullPathToProjectFile);
            fullPathToTargetFile = Path.Combine(context.TestDeploymentDir, fullPathToTargetFile);
        }

        #region Test Methods
        /// <summary>
        ///A test for AddFileFromTemplate (string, string)
        ///</summary>
        [TestMethod()]
        public void AddFileFromTemplateTest()
        {
            NestedProjectNode target = projectNode;
            target.AddFileFromTemplate(fullPathToClassTemplateFile, fullPathToTargetFile);
        }

        /// <summary>
        ///A test for GetAutomationObject ()
        ///</summary>
        [TestMethod()]
        public void GetAutomationObjectTest()
        {
            NestedProjectNode target = projectNode;

            object actual = target.GetAutomationObject();
            Assert.IsNotNull(actual, "Failed to initialize an AutomationObject for "
                + "NestedProjectNode within GetAutomationObject method");
        }

        /// <summary>
        ///A test for GetConfigurationDependentPropertyPages ()
        ///</summary>
        [TestMethod()]
        public void GetConfigurationDependentPropertyPagesTest()
        {
            NestedProjectNode target = projectNode;
            VisualStudio_Project_Samples_NestedProjectNodeAccessor accessor =
                new VisualStudio_Project_Samples_NestedProjectNodeAccessor(target);

            Guid[] expected = new Guid[] { new Guid("C43AD3DC-7468-48e1-B4D2-AAC0C74A0109") };
            Guid[] actual;

            actual = accessor.GetConfigurationDependentPropertyPages();

            CollectionAssert.AreEqual(expected, actual, "Microsoft.VisualStudio.Project.Samples.NestedProject.NestedProjectNode.GetConfigurationDepe" +
                    "ndentPropertyPages did not return the expected value.");
        }

        /// <summary>
        ///A test for GetConfigurationIndependentPropertyPages ()
        ///</summary>
        [TestMethod()]
        public void GetConfigurationIndependentPropertyPagesTest()
        {
            NestedProjectNode target = new NestedProjectNode();
            VisualStudio_Project_Samples_NestedProjectNodeAccessor accessor =
                new VisualStudio_Project_Samples_NestedProjectNodeAccessor(target);


            Guid[] actual;
            actual = accessor.GetConfigurationIndependentPropertyPages();

            Assert.IsTrue(actual != null && actual.Length > 0, "The result of GetConfigurationIndependentPropertyPages was unexpected.");
            Assert.IsTrue(actual[0].Equals(typeof(GeneralPropertyPage).GUID), "The value of collection returned by GetConfigurationIndependentPropertyPages is unexpected.");
        }

        /// <summary>
        ///A test for GetFormatList (out string)
        ///</summary>
        [TestMethod()]
        public void GetFormatListTest()
        {
            NestedProjectNode target = new NestedProjectNode();

            string ppszFormatList;

            int expected = VSConstants.S_OK;
            int actual;

            actual = target.GetFormatList(out ppszFormatList);

            Assert.IsFalse(String.IsNullOrEmpty(ppszFormatList), "[out] ppszFormatList in GetFormatList() method was not set correctly.");
            Assert.AreEqual(expected, actual, "Microsoft.VisualStudio.Project.Samples.NestedProject.NestedProjectNode.GetFormatList did no" +
                    "t return the expected value.");
        }

        /// <summary>
        ///A test for GetPriorityProjectDesignerPages ()
        ///</summary>
        [TestMethod()]
        public void GetPriorityProjectDesignerPagesTest()
        {
            NestedProjectNode target = new NestedProjectNode();
            VisualStudio_Project_Samples_NestedProjectNodeAccessor accessor =
                new VisualStudio_Project_Samples_NestedProjectNodeAccessor(target);


            Guid[] actual;
            actual = accessor.GetPriorityProjectDesignerPages();

            Assert.IsTrue(actual != null && actual.Length > 0, "The result of GetConfigurationIndependentPropertyPages was unexpected.");
            Assert.IsTrue(actual[0].Equals(typeof(GeneralPropertyPage).GUID), "The value of collection returned by GetConfigurationIndependentPropertyPages is unexpected.");
        }

        /// <summary>
        ///A test for NestedProjectNode ()
        ///</summary>
        [TestMethod()]
        public void ConstructorTest()
        {
            NestedProjectNode target = new NestedProjectNode();
            Assert.IsNotNull(target, "Failed to initialize new instance of NestedProjectNode");
        }

        /// <summary>
        ///A test for ProjectGuid
        ///</summary>
        [TestMethod()]
        public void ProjectGuidTest()
        {
            NestedProjectNode target = projectNode;

            Guid val = new Guid(GuidStrings.GuidNestedProjectFactory);

            Assert.AreEqual(val, target.ProjectGuid, "NestedProjectNode.ProjectGuid was not set correctly.");
        }

        /// <summary>
        ///A test for ProjectType
        ///</summary>
        [TestMethod()]
        public void ProjectTypeTest()
        {
            NestedProjectNode target = new NestedProjectNode();

            string val = typeof(NestedProjectNode).Name;

            Assert.AreEqual(val, target.ProjectType, "Microsoft.VisualStudio.Project.Samples.NestedProject.NestedProjectNode.ProjectType was not " +
                    "set correctly.");
        }

        #endregion	
    }
}
