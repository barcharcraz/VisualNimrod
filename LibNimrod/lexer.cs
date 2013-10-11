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

    //we need to layout the struct as we write it
    [StructLayout(LayoutKind.Sequential)]
    public class TLexer
    {
        Int32 fileIdx;
        int indentAhead;
    }
    public class lexer
    {
        [DllImport("libnimrod.dll", EntryPoint="ExpIsKeyword")]
        public static extern bool isKeyword(TokenTypes kind);
    }
}
