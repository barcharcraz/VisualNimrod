using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using NimrodSharp;
namespace NimrodSharp_UintTests
{
    [TestClass]
    public class idetoolstests
    {
        [TestMethod]
        public void TestBasic()
        {
            var filename = Path.GetTempFileName();
            StreamWriter write = File.AppendText(filename);
            write.WriteLine("proc test(a: int) = ");
            write.WriteLine("  echo a");
            write.WriteLine("proc doit() = ");
            write.WriteLine("  test(1)");
            write.Flush();
            write.Close();
            var def = idetoolsfuncs.GetDef(filename, 4, 5, filename);
            Assert.AreEqual(def.type, symTypes.skProc);
        }
    }
}
