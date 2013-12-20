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
        public long iNumber;
        public double fNumber;
        public NumericalBase numBase;
        public IntPtr literal;
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
    public class CToken
    {
        public CToken(IntPtr lex)
        {
            m_token = lexer.rawOpenTok(lex) ;
        }
        ~CToken()
        {
            lexer.freeTok(m_token);
        }
        public TokenTypes type
        {
            get { return token.getTokType(m_token); }
        }
        private IntPtr m_token;
    }
    public class CLexer
    {
        public IntPtr Lexer { get { return m_lex; } }
        public CLexer(string line)
        {
            m_line = new CLLStream(line);
            m_lex = lexer.openLexer("", (IntPtr)m_line);
        }
        public CLexer(CLLStream line)
        {
            m_line = line;
            m_lex = lexer.openLexer("", (IntPtr)line);
        }
        ~CLexer()
        {
            lexer.closeLexer(m_lex);
        }
        public TToken GetNextToken()
        {
            TToken rv = lexer.rawGetTok(m_lex);
            return rv;
        }
        private CLLStream m_line;
        private IntPtr m_lex;
    }
    public static class lexer
    {
        [DllImport("libnimrod.dll", EntryPoint = "ExpOpenLexer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr openLexer([MarshalAs(UnmanagedType.LPStr)] string filename, System.IntPtr llstream);
        [DllImport("libnimrod.dll", EntryPoint = "ExpCloseLexer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void closeLexer(IntPtr lex);
        [DllImport("libnimrod.dll", EntryPoint = "ExpGetLexIndentAhead", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getIndentAhead(IntPtr lex);
        [DllImport("libnimrod.dll", EntryPoint = "ExpRawOpenTok", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr rawOpenTok(IntPtr lex);
        [DllImport("libnimrod.dll", EntryPoint="ExpRawFreeTok", CallingConvention=CallingConvention.Cdecl)]
        public static extern void freeTok(IntPtr tok);
        [DllImport("libnimrod.dll", EntryPoint = "ExpIsKeyword", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isKeyword(TokenTypes kind);
        [DllImport("libnimrod.dll", EntryPoint="ExpRawGetTok", CallingConvention=CallingConvention.Cdecl)]
        public static extern TToken rawGetTok(IntPtr lex);
    }
    public static class token
    {
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetType", CallingConvention = CallingConvention.Cdecl)]
        public static extern TokenTypes getTokType(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetIndent", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getTokIndent(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetiNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern long getTokiNumber(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetfNumber", CallingConvention = CallingConvention.Cdecl)]
        public static extern double getTokfNumber(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetBase", CallingConvention = CallingConvention.Cdecl)]
        public static extern NumericalBase getTokBase(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetLiteral", CallingConvention = CallingConvention.Cdecl)]
        public static extern string getTokLiteral(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetLine", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getTokLine(IntPtr token);
        [DllImport("libnimrod.dll", EntryPoint = "ExpTokGetCol", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getTokCol(IntPtr token);
    }
}
