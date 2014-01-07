using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NimrodSharp.highlite;
using NimrodSharp;
namespace NimrodSharp_UintTests
{
    [TestClass]
    public class HighliteTests
    {
        [TestMethod]
        public void TestCreate()
        {
            string buf = @"proc foo(a:int, b:int):int = ";
            var lexer = highlite.OpenGeneralTokenizer(buf);
            Assert.AreEqual(lexer.kind, TTokenClass.gtEof);
        }
        [TestMethod]
        public void TestClassify()
        {
            string buf = @"proc foo(a:int, b:int):int = ";
            var lexer = highlite.OpenGeneralTokenizer(buf);
            highlite.NimNextToken(ref lexer);
            Assert.AreEqual(lexer.kind, TTokenClass.gtKeyword);
        }
        [TestMethod]
        public void TestNextTok()
        {
            string buf = @"proc foo(a:int, b:int):int = ";
            var lexer = highlite.OpenGeneralTokenizer(buf);
            Assert.AreEqual(lexer.kind, TTokenClass.gtEof);
            highlite.NimNextToken(ref lexer);
            Assert.AreEqual(lexer.kind, TTokenClass.gtKeyword);
        }
    }
}
