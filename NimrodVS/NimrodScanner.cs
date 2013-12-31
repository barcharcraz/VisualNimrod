using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using NimrodSharp;

namespace Company.NimrodVS
{
    class NimrodScanner : IScanner
    {
        private IVsTextBuffer m_buffer;
        private string m_source;
        private CLLStream m_stream;
        private CLexer m_lexer;
        private CToken m_tok;
        public NimrodScanner(IVsTextBuffer buffer)
        {
            m_buffer = buffer;
            
        }
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            
            TokenTypes tokType = m_tok.type;
            switch (tokType)
            {
                case TokenTypes.Invalid:
                case TokenTypes.Eof:
                    return false;
                case TokenTypes.Symbol:
                    tokenInfo.Type = TokenType.Unknown;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TokenTypes.Addr:
                case TokenTypes.And:
                case TokenTypes.As:
                case TokenTypes.Asm:
                case TokenTypes.Atomic:
                case TokenTypes.Bind:
                case TokenTypes.Block:
                case TokenTypes.Break:
                case TokenTypes.Case:
                case TokenTypes.Cast:
                case TokenTypes.Const:
                case TokenTypes.Continue:
                case TokenTypes.Converter:
                case TokenTypes.Discard:
                case TokenTypes.Distinct:
                case TokenTypes.Div:
                case TokenTypes.Do:
                case TokenTypes.Elif:
                case TokenTypes.Else:
                case TokenTypes.End:
                case TokenTypes.Enum:
                case TokenTypes.Except:
                case TokenTypes.Export:
                case TokenTypes.Finally:
                case TokenTypes.For:
                case TokenTypes.From:
                case TokenTypes.Generic:
                case TokenTypes.If:
                case TokenTypes.Import:
                case TokenTypes.In:
                case TokenTypes.Include:
                case TokenTypes.Interface:
                case TokenTypes.Is:
                case TokenTypes.Isnot:
                case TokenTypes.Iterator:
                case TokenTypes.Lambda:
                case TokenTypes.Let:
                case TokenTypes.Macro:
                case TokenTypes.Method:
                case TokenTypes.Using:
                case TokenTypes.Mod:
                case TokenTypes.Nil:
                case TokenTypes.Not:
                case TokenTypes.Notin:
                case TokenTypes.Object:
                case TokenTypes.Of:
                case TokenTypes.Or:
                case TokenTypes.Out:
                case TokenTypes.Proc:
                case TokenTypes.Ptr:
                case TokenTypes.Raise:
                case TokenTypes.Ref:
                case TokenTypes.Return:
                case TokenTypes.Shared:
                case TokenTypes.Shl:
                case TokenTypes.Shr:
                case TokenTypes.Static:
                case TokenTypes.Template:
                case TokenTypes.Try:
                case TokenTypes.Tuple:
                case TokenTypes.Type:
                case TokenTypes.Var:
                case TokenTypes.When:
                case TokenTypes.While:
                case TokenTypes.With:
                case TokenTypes.Without:
                case TokenTypes.Xor:
                case TokenTypes.Yield:
                    tokenInfo.Type = TokenType.Keyword;
                    tokenInfo.Color = TokenColor.Keyword;
                    break;
                case TokenTypes.IntLit:
                case TokenTypes.Int8Lit:
                case TokenTypes.Int16Lit:
                case TokenTypes.Int32Lit:
                case TokenTypes.Int64Lit:
                case TokenTypes.UIntLit:
                case TokenTypes.UInt8Lit:
                case TokenTypes.UInt16Lit:
                case TokenTypes.UInt64Lit:
                case TokenTypes.FloatLit:
                case TokenTypes.Float32Lit:
                case TokenTypes.Float64Lit:
                case TokenTypes.Float128Lit:
                    tokenInfo.Type = TokenType.Literal;
                    tokenInfo.Color = TokenColor.Number;
                    break;
                case TokenTypes.StrLit:
                case TokenTypes.RStrLit:
                case TokenTypes.TripleStrLit:
                case TokenTypes.GStrLit:
                case TokenTypes.GTripleStrLit:
                case TokenTypes.CharLit:
                    tokenInfo.Type = TokenType.String;
                    tokenInfo.Color = TokenColor.String;
                    break;
                case TokenTypes.ParLe:
                case TokenTypes.ParRi:
                case TokenTypes.BraketLe:
                case TokenTypes.BracketRi:
                case TokenTypes.CurlyLe:
                case TokenTypes.CurlyRi:
                case TokenTypes.BracketDotLe:
                case TokenTypes.BracketDotRi:
                case TokenTypes.CurlyDotLe:
                case TokenTypes.CurlyDotRi:
                case TokenTypes.ParDotLe:
                case TokenTypes.ParDotRi:
                    tokenInfo.Type = TokenType.Delimiter;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TokenTypes.Comma:
                case TokenTypes.SemiColon:
                case TokenTypes.Colon:
                case TokenTypes.ColonColon:
                case TokenTypes.Equals:
                case TokenTypes.Dot:
                case TokenTypes.DotDot:
                case TokenTypes.Opr:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case TokenTypes.Comment:
                    tokenInfo.Type = TokenType.Comment;
                    tokenInfo.Color = TokenColor.Comment;
                    break;
                case TokenTypes.Accent:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case TokenTypes.Spaces:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case TokenTypes.InfixOpr:
                case TokenTypes.PrefixOpr:
                case TokenTypes.PostfixOpr:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                default:
                    break;
            }
            return true;
        }

        public void SetSource(string source, int offset)
        {
            m_source = source.Substring(offset);
            m_stream = new CLLStream(m_source);
            m_lexer = new CLexer(m_stream);
            m_tok = new CToken(m_lexer.Lexer);
        }
    }
}
