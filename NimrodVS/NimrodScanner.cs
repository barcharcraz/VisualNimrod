using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;

namespace Company.NimrodVS
{
    class NimrodScanner : IScanner
    {
        private string m_source;
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            throw new NotImplementedException();
        }

        public void SetSource(string source, int offset)
        {
            m_source = source.Substring(offset);

        }
    }
}
