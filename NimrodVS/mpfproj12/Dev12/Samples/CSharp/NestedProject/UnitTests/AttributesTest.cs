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
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject.UnitTests
{
    /// <summary>
    ///This is a test class for VisualStudio.Project.Samples.NestedProject.ResourcesDescriptionAttribute and is intended
    ///to contain all VisualStudio.Project.Samples.NestedProject.ResourcesDescriptionAttribute Unit Tests
    ///</summary>
    [TestClass()]
    public class AttributesTest
    {
        #region Fields
        private TestContext testContextInstance;
        #endregion Fields

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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
        #endregion Properties

        #region The tests for the ResourcesDescriptionAttribute class
        #region Constructors tests
        /// <summary>
        ///The test for ResourcesDescriptionAttribute().
        ///</summary>
        [TestMethod()]
        public void ConstructorTest()
        {
            string description = "Some Attribute Description";

            DescriptionAttribute target = VisualStudio_Project_Samples_ResourcesDescriptionAttributeAccessor.CreatePrivate(description);
            Assert.IsNotNull(target, "ResourcesDescriptionAttribute instance was not created successfully.");
        }
        #endregion Constructors tests

        #region Properties tests
        /// <summary>
        /// The test for Description property
        ///</summary>
        [TestMethod()]
        public void DescriptionAttributeTest()
        {
            string description = "AssemblyName";
            DescriptionAttribute target = VisualStudio_Project_Samples_ResourcesDescriptionAttributeAccessor.CreatePrivate(description);
            Assert.IsNotNull(target, "ResourcesDescriptionAttribute instance was not created successfully.");

            VisualStudio_Project_Samples_ResourcesDescriptionAttributeAccessor accessor = new VisualStudio_Project_Samples_ResourcesDescriptionAttributeAccessor(target);

            Assert.IsNotNull(accessor.Description, "Description property value was uninitialized.");
        }
        #endregion Properties tests
        #endregion The tests for the ResourcesDescriptionAttribute class

        #region The tests for the ResourcesCategoryAttribute class
        #region Constructors tests
        /// <summary>
        ///A test for ResourcesCategoryAttribute (string)
        ///</summary>
        [TestMethod()]
        public void DefaultConstructorTest()
        {
            string category = "AssemblyName";
            CategoryAttribute target = VisualStudio_Project_Samples_ResourcesCategoryAttributeAccessor.CreatePrivate(category);
            Assert.IsNotNull(target, "CategoryAttribute instance was not created successfully.");
        }
        #endregion Constructors tests

        #region Mathod tests
        /// <summary>
        /// The test for GetLocalizedString() method.
        ///</summary>
        [TestMethod()]
        public void GetLocalizedStringTest()
        {
            string category = "AssemblyName";
            CategoryAttribute target = VisualStudio_Project_Samples_ResourcesCategoryAttributeAccessor.CreatePrivate(category);
            VisualStudio_Project_Samples_ResourcesCategoryAttributeAccessor accessor = new VisualStudio_Project_Samples_ResourcesCategoryAttributeAccessor(target);

            string actual = accessor.GetLocalizedString(category);
            Assert.IsNotNull(actual, String.Format("GetLocalizedString() for {0} category was uninitialized.", category));
        }
        #endregion Mathod tests
        #endregion The tests for the ResourcesCategoryAttribute class

        #region The tests for the LocDisplayNameAttribute class
        #region Constructors tests
        /// <summary>
        /// The test for LocDisplayNameAttribute() default constructor.
        ///</summary>
        [TestMethod()]
        public void LocDisplayNameDefaultConstructorTest()
        {
            string name = "AssemblyName";
            DisplayNameAttribute target = VisualStudio_Project_Samples_LocDisplayNameAttributeAccessor.CreatePrivate(name);
            Assert.IsNotNull(target, "DisplayNameAttribute instance was not created successfully.");
        }
        #endregion Constructors tests

        #region Properties tests
        /// <summary>
        /// The test for the DisplayName property.
        ///</summary>
        [TestMethod()]
        public void DisplayNameTest()
        {
            string name = "AssemblyName";
            DisplayNameAttribute target = VisualStudio_Project_Samples_LocDisplayNameAttributeAccessor.CreatePrivate(name);

            Assert.IsNotNull(target.DisplayName, String.Format("DisplayName property for \"{0}\" attribute name was uninitialized.", name));
        }
        /// <summary>
        /// The test for the DisplayName property with not existing corresponding resource string.
        ///</summary>
        [TestMethod()]
        public void DisplayNameWithNotExistingNameResourceStrTest()
        {
            string name = "Some not existing resource string name";
            DisplayNameAttribute target = VisualStudio_Project_Samples_LocDisplayNameAttributeAccessor.CreatePrivate(name);

            Assert.AreEqual(name, target.DisplayName, String.Format("DisplayName property for \"{0}\" attribute name was initialized by unexpected value.", name));
        }
        #endregion Properties tests
        #endregion The tests for the LocDisplayNameAttribute class
    }
}
