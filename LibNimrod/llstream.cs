using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NimrodSharp
{
    /// <summary>
    /// enumerates the kinds of streams
    /// </summary>
    public enum TLLStreamKind
    {
        llsNone,
        llsString,
        llsFile,
        llsStdin
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LLStreamStruct
    {
        public TLLStreamKind kind;
        public System.IntPtr f;
        public string s;
        public int rd;
        public int wr;
        public int lineOffset;

    }
    public class CLLStream
    {
        public TLLStreamKind Kind
        {
            get { return llstream.LLStreamGetKind(stream); }
        }
        public string StrVal
        {
            get 
            {
                return Marshal.PtrToStringAnsi(llstream.LLStreamReadAll(stream));
            }
        }
        public string RawString
        {
            get
            {
                return Marshal.PtrToStringAnsi(llstream.LLStreamGetString(stream));
            }
        }
        public int Rd
        {
            get { return llstream.LLStreamRd(stream); }
        }
        public int Wr
        {
            get { return llstream.LLStreamWr(stream); }
        }
        public CLLStream(string data)
        {
            stream = llstream.LLStreamOpen(data);

        }
        public static explicit operator System.IntPtr(CLLStream toConv)
        {
            return toConv.stream;
        }
        ~CLLStream()
        {
            llstream.LLStreamClose(stream);
        }
        private IntPtr stream;
    }
    /// <summary>
    /// nimrod functions for dealing with streams
    /// </summary>
    public class llstream
    {
        [DllImport("libnimrod.dll", EntryPoint = "ExpStrMarshal", CallingConvention=CallingConvention.Cdecl)]
        public static extern int StrMarshal([MarshalAs(UnmanagedType.LPStr)] string str);
        [DllImport("libnimrod.dll", EntryPoint = "ExpLLStreamOpen", CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr LLStreamOpen([MarshalAs(UnmanagedType.LPStr)] string data);

        [DllImport("libnimrod.dll", EntryPoint = "ExpLLStreamClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LLStreamClose(System.IntPtr stream);

        [DllImport("libnimrod.dll", EntryPoint = "ExpGetLLStreamKind", CallingConvention = CallingConvention.Cdecl)]
        public static extern TLLStreamKind LLStreamGetKind(System.IntPtr stream);
        [DllImport("libnimrod.dll", EntryPoint="ExpGetLLStreamReadAll", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr LLStreamReadAll(System.IntPtr stream);
        [DllImport("libnimrod.dll", EntryPoint = "ExpGetLLStreamString", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LLStreamGetString(IntPtr stream);
        [DllImport("libnimrod.dll", EntryPoint = "ExpGetLLStreamRd", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LLStreamRd(IntPtr stream);
        [DllImport("libnimrod.dll", EntryPoint = "ExpGetLLStreamWr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LLStreamWr(IntPtr stream);
    }
}
