using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NimrodSharp
{
    public static class nimgc
    {
        [DllImport("libnimrod.dll", EntryPoint = "ExpFullcollect", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GC_fullcollect();
        [DllImport("libnimrod.dll", EntryPoint = "ExpGCDisable", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GC_disable();
    }
}
