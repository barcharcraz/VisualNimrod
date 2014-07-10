using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
namespace NimrodProject
{
    public class NimrodProjectNode
    {
        private Package package;

        public NimrodProjectNode(Package package)
        {
            this.package = package;
        }
    }
}
