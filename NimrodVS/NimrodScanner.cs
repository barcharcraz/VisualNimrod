using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using NimrodSharp;

namespace Company.NimrodVS
{
    class NimrodScanner : IScanner
    {
        private string m_source;
        private CLLStream m_stream;
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            
            throw new NotImplementedException();
        }

        public void SetSource(string source, int offset)
        {
            m_source = source.Substring(offset);
            m_stream = new CLLStream(m_source);

        }
    }
}
