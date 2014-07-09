using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
namespace Company.NimrodVS
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
    static class LanguageConstants
    {
        public static readonly HashSet<string> keywords = new HashSet<string>
        {
            "addr", "and", "as", "asm", "atomic",
            "bind", "block", "break",
            "case", "cast", "const", "continue", "converter",
            "discard", "distinct", "div", "do",
            "elif", "else", "end", "enum", "except", "export",
            "finally", "for", "from",
            "generic",
            "if", "import", "in", "include", "interface", "is", "isnot", "iterator",
            "lambda", "let",
            "macro", "method", "mixin", "mod",
            "nil", "not", "notin",
            "object", "of", "or", "out",
            "proc", "ptr",
            "raise", "ref", "return",
            "shl", "shr", "static",
            "template", "try", "tuple", "type",
            "using",
            "var",
            "when", "while", "with", "without",
            "xor",
            "yield"
        };
    }
    class NimrodTokenizer
    {
        private TTokenClass kind;
        private string m_source;
        private int start;
        private int end;
        private string nextToken;
        public int Start { get { return start; } }
        public int End { get { return end; } }
        public string NextToken { get { return nextToken; } }
        public TTokenClass Kind { get { return kind; } }
        public NimrodTokenizer(string source)
        {
            m_source = source;
            start = 0;
            end = 0;
            advanceOne();
        }
        public void advanceOne()
        {
            if (end >= m_source.Length)
            {
                kind = TTokenClass.gtEof;
                return;
            }
            start = end;
            end = m_source.IndexOf(' ', start + 1);
            if (end == -1)
            {
                nextToken = m_source.Substring(start + 1);
                end = m_source.Length;
            }
            nextToken = m_source.Substring(start, end - start);
            kind = TTokenClass.gtOther;
            if (LanguageConstants.keywords.Contains(nextToken))
            {
                kind = TTokenClass.gtKeyword;
            }

        }

    }
    class NimrodScanner : IScanner
    {
        private IVsTextBuffer m_buffer;
        private string m_source;
        //private TGeneralTokenizer m_tokenizer;
        private NimrodTokenizer m_tokenizer;
        public NimrodScanner(IVsTextBuffer buffer)
        {
            m_buffer = buffer;
            
        }
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            var lastToken = m_tokenizer.Kind;
            switch (m_tokenizer.Kind)
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
            tokenInfo.StartIndex = m_tokenizer.Start;
            tokenInfo.EndIndex = m_tokenizer.End;
            m_tokenizer.advanceOne();
            return true;
        }

        public void SetSource(string source, int offset)
        {
            m_source = source.Substring(offset);
            m_tokenizer = new NimrodTokenizer(m_source);
        }
    }
}
