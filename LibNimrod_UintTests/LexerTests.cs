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
            lex = lexer.openLexer((IntPtr)line);
            Assert.AreEqual(lexer.getIndentAhead(lex), 2);
            lexer.closeLexer(lex);
        }
        [TestMethod]
        public void parseType()
        {
            CLexer lex = new CLexer(@"proc foo(a: int, b: int):int =");
            var tok = lex.GetNextCToken();
            Assert.AreEqual(tok.type, TokenTypes.Proc);

        }
        [TestMethod]
        public void getLiteral()
        {
            CLexer lex = new CLexer(@"proc foo(a: int, b: int):int =");
            var tok = lex.GetNextCToken();
            Assert.AreEqual(tok.ParsedString, "proc");
        }
        [TestMethod]
        public void streamOpen()
        {
            var stream = new CLLStream(@"import llstream");
            var lexer = new CLexer(stream);
            var tok = lexer.GetNextCToken();
            Assert.AreEqual("import", tok.ParsedString);
        }
        [TestMethod]
        public void gctests()
        {
            var stream = new CLLStream(@"import lexer");
            nimgc.GC_fullcollect();
            var lexer = new CLexer(stream);
            nimgc.GC_fullcollect();
            var tok = lexer.GetNextCToken();
            nimgc.GC_fullcollect();
            Assert.AreEqual("import", tok.ParsedString);
            nimgc.GC_fullcollect();
            tok = lexer.GetNextCToken();
            nimgc.GC_fullcollect();
            Assert.AreEqual("lexer", tok.ParsedString);
            nimgc.GC_fullcollect();
            tok = lexer.GetNextCToken();
            Assert.AreEqual(TokenTypes.Eof, tok.type);
            nimgc.GC_fullcollect();
            stream = new CLLStream(@"import llstream");
            nimgc.GC_fullcollect();
            lexer = new CLexer(stream);
            nimgc.GC_fullcollect();
            tok = lexer.GetNextCToken();
            nimgc.GC_fullcollect();
            Assert.AreEqual("import", tok.ParsedString);

        }
        [TestMethod]
        public void repeatTest()
        {
            string line = @"proc ExpGetLLStreamReadAll(s: PLLStream): cstring {.cdecl, exportc, dynlib.} =";
            //nimgc.GC_disable();
            //var stream = new CLLStream(line);
            for (int i = 0; i < 100; i++)
            {
                var stream = new CLLStream(line);
                var lexer = new CLexer(stream);
                var tok = lexer.GetNextCToken();
                while (tok.type != TokenTypes.Eof)
                {
                    tok = lexer.GetNextCToken();
                }
            }
        }
    }
    [TestClass]
    public class FinalizerTests
    {
        class tclass
        {
            public int isAlive = 10;
            ~tclass()
            {
                isAlive = 0;
            }
        }
        [TestMethod]
        public void finalizerTest()
        {
            for (int i = 0; i < 100; ++i)
            {
                new tclass();
            }
        }
    }
}
