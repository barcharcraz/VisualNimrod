using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using NimrodSharp;
namespace LibNimrod_UintTests
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void isKeywordTestTrue()
        {
            TokenTypes test = TokenTypes.Let;
            Assert.IsTrue(lexer.isKeyword(test));
        }

        [TestMethod]
        public void isKeywordTestFalse()
        {
            TokenTypes test = TokenTypes.Int8Lit;
            Assert.IsFalse(lexer.isKeyword(test));
        }
        [TestMethod]
        public void canOpen()
        {
            TLexer lex = new TLexer();
            CLLStream line = new CLLStream(@"proc foo(a: int, b:int):int =");
            lexer.openLexer(ref lex, "", (IntPtr)line);
            Assert.AreEqual(lex.indentAhead, 2);
            lexer.closeLexer(ref lex);
        }
        [TestMethod]
        public void parseType()
        {
            CLexer lex = new CLexer(@"proc foo(a: int, b: int):int =");
            var tok = lex.GetNextToken();
            Assert.AreEqual(tok.tokType, TokenTypes.Proc);

        }
    }
}
