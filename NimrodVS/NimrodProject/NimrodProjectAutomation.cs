using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project.Automation;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
namespace Company.NimrodVS.NimrodProject
{
    [ComVisible(true)]
    public class OANimrodProject : OAProject
    {
        public OANimrodProject(NimrodProjectNode project)
            : base(project)
        {

        }
    }
    [ComVisible(true)]
    public class OANimrodProjectFileItem : OAFileItem
    {
        public OANimrodProjectFileItem(OAProject project, FileNode node)
            : base(project, node)
        {

        }
    }
}
