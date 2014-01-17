using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NimrodSharp
{
    public class idetools
    {
        private Process nimrodProc;
        public idetools()
        {
            nimrodProc = Process.Start(CreateStartInfo());
        }
    }
    public static class idetools
    {
        private static ProcessStartInfo CreateStartInfo(string )
        {
            ProcessStartInfo rv = new ProcessStartInfo("nimrod");
            rv.Arguments("serve --server.type:stdin")
            rv.CreateNoWindow = true;
            rv.RedirectStandardError = true;
            rv.RedirectStandardInput = true;
            rv.RedirectStandardOutput = true;
            return rv;
        }
    }
}
