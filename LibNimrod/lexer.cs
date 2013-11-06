using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NimrodSharp
{
    /// <summary>
    /// enumerates the types of tokens,
    /// this is the TTokType enum from
    /// lexer.nim
    /// </summary>
    public enum TokenTypes
    {
        Invalid,
        Eof,
        Symbol,
        Addr,
        And,
        As,
        Asm,
        Atomic,
        Bind,
        Block,
        Break,
        Case,
        Cast,
        Const,
        Continue,
        Converter,
        Discard,
        Distinct,
        Div,
        Do,
        Elif,
        Else,
        End, 
        Enum,
        Except,
        Export,
        Finally,
        For, 
        From,
        Generic,
        If,
        Import,
        In,
        Include,
        Interface,
        Is,
        Isnot,
        Iterator,
        Lambda,
        Let,
        Macro,
        Method,
        Mixin,
        Using,
        Mod,
        Nil,
        Not,
        Notin,
        Object,
        Of,
        Or,
        Out,
        Proc,
        Ptr,
        Raise,
        Ref,
        Return,
        Shared,
        Shl,
        Shr,
        Static,
        Template,
        Try,
        Tuple,
        Type,
        Var,
        When,
        While,
        With,
        Without,
        Xor,
        Yield,
        IntLit,
        Int8Lit,
        Int16Lit,
        Int32Lit,
        Int64Lit,
        UIntLit,
        UInt8Lit,
        UInt16Lit,
        UInt64Lit,
        FloatLit,
        Float32Lit,
        Float64Lit,
        Float128Lit,
        StrLit,
        RStrLit,
        TripleStrLit,
        GStrLit,
        GTripleStrLit,
        CharLit,
        ParLe,
        ParRi,
        BraketLe,
        BracketRi,
        CurlyLe,
        CurlyRi,
        BracketDotLe,
        BracketDotRi,
        CurlyDotLe,
        CurlyDotRi,
        ParDotLe,
        ParDotRi,
        Comma,
        SemiColon,
        Colon,
        ColonColon,
        Equals,
        Dot,
        DotDot,
        Opr,
        Comment,
        Accent,
        Spaces,
        InfixOpr,
        PrefixOpr,
        PostfixOpr
    }
    public enum NumericalBase
    {
        base10,
        base2,
        base8,
        base16
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TToken
    {
        public TokenTypes tokType;
        public int indent;
        public System.IntPtr ident;
        public long iNumber;
        public double fNumber;
        public NumericalBase numBase;
        [MarshalAs(Un)]
        public string literal;
        public int line;
        public int col;
    }
    //we need to layout the struct as we write it
    [StructLayout(LayoutKind.Sequential)]
    public struct TLexer
    {
        //base class
        public int bufpos;
        [MarshalAs(UnmanagedType.LPStr)]
        public string buf;
        public int bufLen;
        public IntPtr stream;
        public int LineNumber;
        public int sentinel;
        public int lineStart;
        //derived class
        public Int32 fileIdx;
        public int indentAhead;
    }
    public class CLexer
    {
        public CLexer(string line)
        {
            m_line = new CLLStream(line);
            lexer.openLexer(ref m_lex, "", (IntPtr)m_line);
        }
        public CLexer(CLLStream line)
        {
            m_line = line;
            lexer.openLexer(ref m_lex, "", (IntPtr)line);
        }
        ~CLexer()
        {
            lexer.closeLexer(ref m_lex);
        }
        public TToken GetNextToken()
        {
            TToken rv = new TToken();
            lexer.rawGetTok(ref m_lex, ref rv);
            return rv;
        }
        private CLLStream m_line;
        private TLexer m_lex;
    }
    public static class lexer
    {
        [DllImport("libnimrod.dll", EntryPoint = "ExpOpenLexer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void openLexer(ref TLexer lex, [MarshalAs(UnmanagedType.LPStr)] string filename, System.IntPtr llstream);
        [DllImport("libnimrod.dll", EntryPoint = "ExpCloseLexer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void closeLexer(ref TLexer lex);
        [DllImport("libnimrod.dll", EntryPoint = "ExpRawGetTok", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rawGetTok(ref TLexer l, ref TToken tok);
        [DllImport("libnimrod.dll", EntryPoint = "ExpIsKeyword", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isKeyword(TokenTypes kind);
    }
}
