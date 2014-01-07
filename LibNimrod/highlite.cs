using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace NimrodSharp
{
    namespace highlite
    {
        public enum TTokenClass : int
        {
            gtEof, gtNone, gtWhitespace, gtDecNumber, gtBinNumber, gtHexNumber,
            gtOctNumber, gtFloatNumber, gtIdentifier, gtKeyword, gtStringLit,
            gtLongStringLit, gtCharLit, gtEscapeSequence,
            gtOperator, gtPunctation, gtComment, gtLongComment, gtRegularExpression,
            gtTagStart, gtTagEnd, gtKey, gtValue, gtRawData, gtAssembler,
            gtPreprocessor, gtDirective, gtCommand, gtRule, gtHyperlink, gtLabel,
            gtReference, gtOther
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct TGeneralTokenizer
        {
            public TTokenClass kind;
            public int start;
            public int length;
            public IntPtr buf;
            public int pos;
            public TTokenClass state;
        }
        public static class highlite
        {
            [DllImport("libnimrod.dll", EntryPoint = "ExpColorOpenGeneralTokenizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void OpenGeneralTokenizer(IntPtr tokenizer, IntPtr buf);
            [DllImport("libnimrod.dll", EntryPoint="ExpColorNimNextToken", CallingConvention=CallingConvention.Cdecl)]
            public static extern void NimNextToken(ref TGeneralTokenizer tokenizer);

            public static unsafe TGeneralTokenizer OpenGeneralTokenizer(string buf)
            {
                TGeneralTokenizer retval;
                
                IntPtr nativeBuf = IntPtr.Zero;
                try
                {
                    byte[] strbuf = Encoding.UTF8.GetBytes(buf);
                    nativeBuf = Marshal.AllocHGlobal(strbuf.Length + 1);
                    Marshal.Copy(strbuf, 0, nativeBuf, strbuf.Length);
                    Marshal.WriteByte(nativeBuf + strbuf.Length, 0);
                }
                catch (Exception)
                {
                    if (nativeBuf != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(nativeBuf);
                    }
                    throw;
                }
                IntPtr rv = (IntPtr)((void*)&retval);
                highlite.OpenGeneralTokenizer(rv, nativeBuf);
                return retval;
            }
            public static unsafe void CloseGeneralTokenizer(ref TGeneralTokenizer tokenizer)
            {
                if (tokenizer.buf != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(tokenizer.buf);
                }
            }
        }
    }
    
}
