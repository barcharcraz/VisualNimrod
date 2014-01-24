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
            nimrodProc = Process.Start(idetoolsfuncs.CreateStartInfo());
        }
    }
    public static class idetoolsfuncs
    {
        public static ProcessStartInfo CreateStartInfo()
        {
            ProcessStartInfo rv = new ProcessStartInfo("nimrod");
            rv.Arguments = "serve --server.type:stdin";
            rv.CreateNoWindow = true;
            rv.RedirectStandardError = true;
            rv.RedirectStandardInput = true;
            rv.RedirectStandardOutput = true;
            return rv;
        }
    }
}
