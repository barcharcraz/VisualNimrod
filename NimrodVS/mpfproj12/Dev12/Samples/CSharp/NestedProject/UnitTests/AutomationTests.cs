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
using MSBuild = Microsoft.Build.BuildEngine;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject.UnitTests
{
    /// <summary>
    ///This is a test class for VisualStudio.Project.Samples.NestedProject.OANestedProject and is intended
    ///to contain all VisualStudio.Project.Samples.NestedProject.OANestedProject Unit Tests.
    ///</summary>
    [TestClass()]
    public class AutomationTests : BaseTest
    {
        #region Fields

        private OANestedProject nestedProject;
        private OANestedProjectProperties projectProperties;
        #endregion Fields

        #region Tests Initialization && Cleanup
        [ClassInitialize]
        public static void TestClassInitialize(TestContext context)
        {
            fullPathToClassTemplateFile = Path.Combine(context.TestDeploymentDir, fullPathToClassTemplateFile);
            fullPathToProjectFile = Path.Combine(context.TestDeploymentDir, fullPathToProjectFile);
            fullPathToTargetFile = Path.Combine(context.TestDeploymentDir, fullPathToTargetFile);
        }

        /// <summary>
        /// Runs before the test to allocate and configure resources needed 
        /// by all tests in the test class.
        /// </summary>
        [TestInitialize()]
        public override void Initialize()
        {
            base.Initialize();

            //init the automation objects
            nestedProject = new OANestedProject(projectNode);
            projectProperties = (OANestedProjectProperties)nestedProject.Properties;
        }
        #endregion

        #region The tests for the OANestedProject && OANestedProjectProperties classes
        #region Constructors tests
        /// <summary>
        /// The test for OANestedProject default constructor.
        ///</summary>
        [TestMethod()]
        public void ConstructorTest()
        {
            Assert.IsNotNull(nestedProject, "OANestedProject instance was uninitialized.");
            Assert.IsNotNull(nestedProject.Project, "OANestedProject Project property was uninitialized.");
        }
        #endregion Constructors tests

        #region Properties tests
        /// <summary>
        /// The test for Properties property.
        ///</summary>
        [TestMethod()]
        public void PropertiesTest()
        {
            Assert.IsNotNull(nestedProject.Properties, "Node Properties was uninitialized.");
            Assert.IsTrue((nestedProject.Properties is OANestedProjectProperties),
                "Returned Node Properties was initialized by unexpected type value.");
        }
        #endregion Properties tests
        #endregion The tests for the OANestedProject && OANestedProjectProperties classes

        #region The tests for the OANestedProjectProperty class

        #region Constructors tests
        /// <summary>
        /// The test for OANestedProjectProperty explicit default constructor.
        ///</summary>
        [TestMethod()]
        public void DefaultConstructorTest()
        {
            OANestedProjectProperty target = new OANestedProjectProperty();
            Assert.IsNotNull(target, "The OANestedProjectProperty instance was not created successfully.");
        }

        /// <summary>
        /// The test for OANestedProjectProperty internal constructor.
        ///</summary>
        [TestMethod()]
        public void InternalConstructorTest()
        {

            Assert.IsNotNull(nestedProject.Properties, "Node Properties was uninitialized.");
            Assert.IsTrue((nestedProject.Properties is OANestedProjectProperties), "Returned Node Properties was initialized by unexpected type value.");

            string name = "Some random name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
        }
        #endregion Constructors tests

        #region Properties tests

        /// <summary>
        /// The test for the Application property.
        /// </summary>
        /// <remarks>This property marked as "Microsoft Internal Use Only" and returns null.</remarks>
        [TestMethod()]
        public void ApplicationPropertyTest()
        {
            string name = "Some random name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
            Assert.IsNull(testProperty.Application, "Application property was returned as initialized value.");
        }
        /// <summary>
        /// The test for the Parent property.
        /// </summary>
        [TestMethod()]
        public void ParentPropertyTest()
        {
            string name = "Some random name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
            Assert.AreEqual(projectProperties, testProperty.Parent, "ProjectProperty Parent was initialized by unexpected value.");
        }
        /// <summary>
        /// The test for the Collection property.
        /// </summary>
        [TestMethod()]
        public void CollectionPropertyTest()
        {
            string name = "Some random name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
            Assert.AreEqual(projectProperties, testProperty.Collection, "ProjectProperty Collection was initialized by unexpected value.");
        }
        /// <summary>
        /// The test for the Parent DTE property.
        /// </summary>
        [TestMethod()]
        public void ParentDTEPropertyTest()
        {
            string name = "Some random name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
            Assert.AreEqual(projectProperties.DTE, testProperty.DTE, "ProjectProperty Parent.DTE was initialized by unexpected value.");
        }
        /// <summary>
        /// The test for the Name property.
        /// </summary>
        [TestMethod()]
        public void NamePropertyTest()
        {
            string name = "Some random name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
            Assert.AreEqual(name, testProperty.Name, "ProjectProperty Name was initialized by unexpected value.");
        }
        /// <summary>
        /// The test for the get_IndexValue() method.
        /// </summary>
        /// <remarks>Probably method get_IndexValue() is uncompleted.</remarks>
        [TestMethod()]
        public void get_IndexedValueTest()
        {
            string name = "Some Random Name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");

            object actualValue = testProperty.get_IndexedValue(null, null, null, null);
            Assert.IsNull(actualValue, "Method get_IndexValue was returned unexpected value.");
        }
        /// <summary>
        /// The test for the set_IndexValue() method.
        /// </summary>
        /// <remarks>Probably method set_IndexValue() is uncompleted.</remarks>
        [TestMethod()]
        public void set_IndexedValueTest()
        {
            string name = "Some Random Name";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");

            // simply call this method
            testProperty.set_IndexedValue(null, null, null, null, null);
        }
        /// <summary>
        /// This method tests Object and dependent on Value properties.
        /// </summary>
        [TestMethod()]
        public void ObjectAndValuePropertiesTest()
        {
            string name = "SomeRandomName";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");

            testProperty.Object = name;

            Assert.AreEqual((object)name, testProperty.Object, "ProjectProperty Object was initialized by unexpected value.");
            Assert.AreEqual((object)name, testProperty.Value, "ProjectProperty Value was initialized by unexpected value.");
            Assert.IsTrue(nestedProject.IsDirty, "After property changing IsDirty flag was not set to the false");
        }
        /// <summary>
        /// The test for the Value property in scenario when assigned to integer value.
        /// </summary>
        [TestMethod()]
        public void ValueAsIntegerPropertyTest()
        {
            string name = "SomeRandomName";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");

            int expectedValue = 77777;
            testProperty.Value = expectedValue;

            Assert.AreEqual(expectedValue.ToString(), testProperty.Value, "ProjectProperty Value was initialized by unexpected value.");
            Assert.IsTrue(nestedProject.IsDirty, "After property changing IsDirty flag was not set to the false");
        }
        /// <summary>
        /// The test for the Value property in scenario when assigned to null referenced value.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValueAsNullPropertyTest()
        {
            string name = "SomeRandomName";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");

            testProperty.Value = null;
        }
        /// <summary>
        /// The test for the let_Value method.
        /// </summary>
        [TestMethod()]
        public void let_ValueMethodTest()
        {
            string name = "SomeRandomName";
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");

            int expectedValue = 77777;
            testProperty.let_Value(expectedValue);

            Assert.AreEqual(expectedValue.ToString(), testProperty.Value, "ProjectProperty Value was initialized by unexpected value.");
            Assert.IsTrue(nestedProject.IsDirty, "After property changing IsDirty flag was not set to the false");
        }
        /// <summary>
        /// The test method for the NumIndices property.
        /// </summary>
        /// <remarks>This property always returns zero value.</remarks>
        [TestMethod()]
        public void NumIndicesPropertyTest()
        {
            string name = "Some Random Name";
            short expectedValue = 0;
            OANestedProjectProperty testProperty = VisualStudio_Project_Samples_OANestedProjectPropertyAccessor.CreatePrivate(projectProperties, name);
            Assert.IsNotNull(testProperty, "The OANestedProjectProperty instance was not created successfully.");
            Assert.AreEqual(expectedValue, testProperty.NumIndices, "Property NumIndices was returned unexpected value.");
        }
        #endregion Properties tests

        #endregion The tests for the OANestedProjectProperty class
    }
}
