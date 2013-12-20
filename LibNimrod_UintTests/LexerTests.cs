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
            IntPtr lex;
            CLLStream line = new CLLStream(@"proc foo(a: int, b:int):int =");
            lex = lexer.openLexer("", (IntPtr)line);
            Assert.AreEqual(lexer.getIndentAhead(lex), 2);
            lexer.closeLexer(lex);
        }
        [TestMethod]
        public void parseType()
        {
            CLexer lex = new CLexer(@"proc foo(a: int, b: int):int =");
            var tok = new CToken(lex.Lexer);
            Assert.AreEqual(tok.type, TokenTypes.Proc);

        }
    }
}
