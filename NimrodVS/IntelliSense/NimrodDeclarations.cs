using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using NimrodSharp;
namespace Company.NimrodVS.IntelliSense
{
    public class NimrodDeclarations : Declarations
    {
        private List<idetoolsReply> m_decl;
        public NimrodDeclarations(List<idetoolsReply> reply)
            : base()
        {
            m_decl = reply;
        }
        public override int GetCount()
        {
            return m_decl.Count;
        }

        public override string GetDescription(int index)
        {
            if (index >= m_decl.Count)
            {
                return "";
            }
            return m_decl[index].docstring;
        }

        public override string GetDisplayText(int index)
        {
            if (index >= m_decl.Count)
            {
                return "";
            }
            return m_decl[index].path;
        }

        public override int GetGlyph(int index)
        {
            return 0;
        }

        public override string GetName(int index)
        {
            if (index >= m_decl.Count)
            {
                return "";
            }
            var nameWOModule = m_decl[index].path.Split('.').Last();
            return nameWOModule;
        }
    }
}
