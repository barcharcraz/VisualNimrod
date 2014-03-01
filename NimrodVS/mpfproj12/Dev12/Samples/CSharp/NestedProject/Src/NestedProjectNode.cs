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
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject
{
    /// <summary>
    /// This class extends the ContainerNode in order to represent our project 
    /// within the hierarchy.
    /// </summary>
    [GuidAttribute(GuidStrings.GuidNestedProjectNode)]
    public class NestedProjectNode : ProjectContainerNode
    {
        #region Constructors
        /// <summary>
        /// Explicitly defined default constructor.
        /// </summary>
        public NestedProjectNode()
        {
            this.SupportsProjectDesigner = true;
            this.CanProjectDeleteItems = true;

            // Add Category IDs mapping in order to support properties for project items
            AddCATIDMapping(typeof(FileNodeProperties), typeof(FileNodeProperties).GUID);
            AddCATIDMapping(typeof(ProjectNodeProperties), typeof(ProjectNodeProperties).GUID);
            AddCATIDMapping(typeof(FolderNodeProperties), typeof(FolderNodeProperties).GUID);
            AddCATIDMapping(typeof(ReferenceNodeProperties), typeof(ReferenceNodeProperties).GUID);
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets project's Guid value.
        /// </summary>
        public override Guid ProjectGuid
        {
            get
            {
                return typeof(NestedProjectFactory).GUID;
            }
        }
        /// <summary>
        /// Gets project's type as string value.
        /// </summary>
        public override string ProjectType
        {
            get
            {
                return this.GetType().Name;
            }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Generate new Guid value and update it with GeneralPropertyPage GUID.
        /// </summary>
        /// <returns>Returns the property pages that are independent of configuration.</returns>
        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }

        /// <summary>
        /// Overriding to provide project general property page.
        /// </summary>
        /// <returns>Returns the GeneralPropertyPage GUID value.</returns>
        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }

        /// <summary>
        /// Specify here a property page. 
        /// By returning no property page the configuration dependent properties will be neglected.
        /// </summary>
        /// <returns>Returns the configuration dependent property pages.</returns>
        protected override Guid[] GetConfigurationDependentPropertyPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(NestedProjectBuildPropertyPage).GUID;
            return result;
        }

        /// <summary>
        /// Overriding to provide customization of files on add files.
        /// This will replace tokens in the file with actual value (namespace, class name,...)
        /// </summary>
        /// <param name="source">Full path to template file.</param>
        /// <param name="target">Full path to destination file.</param>
        /// <exception cref="FileNotFoundException">Template file is not founded.</exception>
        public override void AddFileFromTemplate(string source, string target)
        {
            if(!File.Exists(source))
            {
                throw new FileNotFoundException(string.Format("Template file not found: {0}", source));
            }

            // The class name is based on the new file name
            string fileName = Path.GetFileNameWithoutExtension(target);
            string nameSpace = this.FileTemplateProcessor.GetFileNamespace(target, this);

            this.FileTemplateProcessor.Reset();
            this.FileTemplateProcessor.AddReplace("%className%", fileName);
            this.FileTemplateProcessor.AddReplace("%namespace%", nameSpace);

            try
            {
                this.FileTemplateProcessor.UntokenFile(source, target);

            }
            catch(Exception exceptionObj)
            {
                throw new FileLoadException(Resources.ResourceManager.GetString("MsgFailedToLoadTemplateFile"), target, exceptionObj);
            }
        }

        /// <summary>
        /// Creates the format list for the open file dialog.
        /// </summary>
        /// <param name="ppszFormatList">The format list to return.</param>
        /// <returns>S_OK if method is succeeded.</returns>
        public override int GetFormatList(out string ppszFormatList)
        {
            ppszFormatList = String.Format(CultureInfo.CurrentCulture, Resources.GetString(Resources.NestedProjectFileAssemblyFilter), "\0", "\0");
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Adds support for project properties.
        /// </summary>
        /// <returns>Return the automation object associated to this project.</returns>
        public override object GetAutomationObject()
        {
            return new OANestedProject(this);
        }

        #endregion Methods
    }
}
