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
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project.Automation;

namespace Microsoft.VisualStudio.Project.Samples.NestedProject
{
    /// <summary>
    /// This class provides automation support for ProjectNode.
    /// </summary>
    /// <remarks>This class has public scope in order for COM to recognize this class</remarks>
    [ComVisible(true)]
    public class OANestedProjectProperty : EnvDTE.Property
    {
        #region Fields
        private OAProperties parent;
        private string name = String.Empty;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Default public constructor for Com visibility.
        /// </summary>
        public OANestedProjectProperty()
        {
        }
        /// <summary>
        /// Initializes new instance of OANestedProjectProperty object based on specified 
        /// parent ProjectNode and Property name.
        /// </summary>
        /// <param name="parent">Parent properties collection.</param>
        /// <param name="name">Project property name.</param>
        internal OANestedProjectProperty(OANestedProjectProperties parent, string name)
        {
            this.parent = parent;
            this.name = name;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Microsoft Internal Use Only.
        /// </summary>
        public object Application
        {
            get { return null; }
        }
        /// <summary>
        /// Gets the Collection containing the Property object supporting this property.
        /// </summary>
        public EnvDTE.Properties Collection
        {
            get
            {
                return this.parent;
            }
        }
        /// <summary>
        /// Gets the top-level extensibility object.
        /// </summary>
        public EnvDTE.DTE DTE
        {
            get
            {
                return this.parent.DTE;
            }
        }
        /// <summary>
        /// Returns one element of a list. 
        /// </summary>
        /// <param name="Index1">The index of the item to display.</param>
        /// <param name="Index2">The index of the item to display. Reserved for future use.</param>
        /// <param name="Index3">The index of the item to display. Reserved for future use.</param>
        /// <param name="Index4">The index of the item to display. Reserved for future use.</param>
        /// <returns>The value of a property</returns>
        // The message is suppressed to follow the csharp naming conventions instead of the base's naming convention that is using c++
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        public object get_IndexedValue(object Index1, object Index2, object Index3, object Index4)
        {
            return null;
        }

        /// <summary>
        /// Setter function to set properties values. 
        /// </summary>
        /// <param name="lppvReturn"></param>
        public void let_Value(object lppvReturn)
        {
            this.Value = lppvReturn;
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the number of indices required to access the value.
        /// </summary>
        public short NumIndices
        {
            get { return 0; }
        }

        /// <summary>
        /// Sets or gets the object supporting the Property object.
        /// </summary>
        // The message is suppressed to follow the csharp naming conventions instead of the base's naming convention that is using c++
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        public object Object
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Microsoft Internal Use Only.
        /// </summary>
        public EnvDTE.Properties Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        /// Sets the value of the property at the specified index.
        /// </summary>
        /// <param name="Index1">The index of the item to set.</param>
        /// <param name="Index2">Reserved for future use.</param>
        /// <param name="Index3">Reserved for future use.</param>
        /// <param name="Index4">Reserved for future use.</param>
        /// <param name="value">The value to set.</param>
        public void set_IndexedValue(object Index1, object Index2, object Index3, object Index4, object Val)
        {
        }

        /// <summary>
        /// Gets or sets the value of the property returned by the Property object.
        /// </summary>
        public object Value
        {
            get
            {
                return this.parent.Target.Node.ProjectMgr.GetProjectProperty(this.name);
            }
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if(value is string)
                {
                    this.parent.Target.Node.ProjectMgr.SetProjectProperty(this.name, value.ToString());
                }
                else
                {
                    this.parent.Target.Node.ProjectMgr.SetProjectProperty(this.name, value.ToString());
                }
            }
        }
        #endregion Properties
    }
}
