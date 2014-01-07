using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using NimrodSharp;
using NimrodSharp.highlite;
namespace Company.NimrodVS
{
    class NimrodScanner : IScanner
    {
        private IVsTextBuffer m_buffer;
        private string m_source;
        private TGeneralTokenizer m_tokenizer;
        public NimrodScanner(IVsTextBuffer buffer)
        {
            m_buffer = buffer;
            
        }
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            highlite.NimNextToken(ref m_tokenizer);
            switch (m_tokenizer.kind)
            {
                case TTokenClass.gtEof:
                    return false;
                case TTokenClass.gtNone:
                    tokenInfo.Type = TokenType.Unknown;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TTokenClass.gtWhitespace:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TTokenClass.gtDecNumber:
                case TTokenClass.gtBinNumber:
                case TTokenClass.gtHexNumber:
                case TTokenClass.gtOctNumber:
                case TTokenClass.gtFloatNumber:
                    tokenInfo.Type = TokenType.Literal;
                    tokenInfo.Color = TokenColor.Number;
                    break;
                case TTokenClass.gtIdentifier:
                    tokenInfo.Type = TokenType.Identifier;
                    tokenInfo.Color = TokenColor.Identifier;
                    break;
                case TTokenClass.gtKeyword:
                    tokenInfo.Type = TokenType.Keyword;
                    tokenInfo.Color = TokenColor.Keyword;
                    break;
                case TTokenClass.gtStringLit:
                case TTokenClass.gtLongStringLit:
                case TTokenClass.gtCharLit:
                    tokenInfo.Type = TokenType.String;
                    tokenInfo.Color = TokenColor.String;
                    break;
                case TTokenClass.gtEscapeSequence:
                    tokenInfo.Type = TokenType.Unknown;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TTokenClass.gtOperator:
                    tokenInfo.Type = TokenType.Operator;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TTokenClass.gtPunctation:
                    tokenInfo.Type = TokenType.Text;
                    tokenInfo.Color = TokenColor.Text;
                    break;
                case TTokenClass.gtComment:
                    tokenInfo.Type = TokenType.Comment;
                    tokenInfo.Color = TokenColor.Comment;
                    break;
                case TTokenClass.gtLongComment:
                    tokenInfo.Type = TokenType.LineComment;
                    tokenInfo.Color = TokenColor.Comment;
                    break;
                case TTokenClass.gtRegularExpression:
                case TTokenClass.gtTagStart:
                case TTokenClass.gtTagEnd:
                case TTokenClass.gtKey:
                case TTokenClass.gtValue:
                case TTokenClass.gtRawData:
                case TTokenClass.gtAssembler:
                case TTokenClass.gtPreprocessor:
                case TTokenClass.gtDirective:
                case TTokenClass.gtCommand:
                case TTokenClass.gtRule:
                case TTokenClass.gtHyperlink:
                case TTokenClass.gtLabel:
                case TTokenClass.gtReference:
                case TTokenClass.gtOther:
                    tokenInfo.Type = TokenType.Unknown;
                    tokenInfo.Color = TokenColor.Text;
                    break;
            }
            tokenInfo.StartIndex = m_tokenizer.start -1;
            tokenInfo.EndIndex = m_tokenizer.start + m_tokenizer.length - 1;
            return true;
        }

        public void SetSource(string source, int offset)
        {
            highlite.CloseGeneralTokenizer(ref m_tokenizer);
            m_source = source.Substring(offset);
            m_tokenizer = highlite.OpenGeneralTokenizer(m_source);
        }
    }
}
