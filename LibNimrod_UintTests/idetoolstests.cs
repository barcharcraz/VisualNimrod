using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NimrodSharp;
namespace NimrodSharp_UintTests
{
    [TestClass]
    public class idetoolstests
    {
        private static string generateNimrodTestFile()
        {
            var filename = Path.GetTempFileName();
            StreamWriter write = File.AppendText(filename);
            write.WriteLine("proc test(a: int) = ");
            write.WriteLine("  echo a");
            write.WriteLine("proc doit() = ");
            write.WriteLine("  test(1)");
            write.Flush();
            write.Close();
            return filename;
        }
        private static string generateNimrodDirtyFile()
        {
            var filename = Path.GetTempFileName();
            StreamWriter write = File.AppendText(filename);
            write.WriteLine("proc test(a: int) = ");
            write.WriteLine("  echo a");
            write.WriteLine("proc doit() = ");
            write.WriteLine("  tes");
            write.Flush();
            write.Close();
            return filename;
        }
        [TestMethod]
        public void TestDef()
        {
            var filename = generateNimrodTestFile();
            var def = idetoolsfuncs.GetDef(filename, 4, 5, filename);
            Assert.AreEqual(def.type, symTypes.skProc);
        }
        [TestMethod]
        public void TestSuggest()
        {
            var filename = generateNimrodTestFile();
            var suggs = idetoolsfuncs.GetSuggestions(filename, 4, 5, filename);
            Assert.AreEqual(suggs.Count, 2);
            Assert.AreEqual(suggs[0].type, symTypes.skProc);
        }
        [TestMethod]
        public void TestDirtySuggest()
        {
            var filename = generateNimrodDirtyFile();
            var suggs = idetoolsfuncs
        }
    }
}
