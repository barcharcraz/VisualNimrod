using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
