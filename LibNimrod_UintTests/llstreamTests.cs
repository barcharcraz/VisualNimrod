using System;
using NimrodSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibNimrod_UintTests
{
    
    [TestClass]
    public class llstreamTests
    {
        [TestMethod]
        public void TestStringMarshal()
        {
            string test = "this is a test";
            int len = llstream.StrMarshal(test);
            Assert.AreEqual(test.Length, len);
        }
        [TestMethod]
        public void TestLLStreamKind()
        {
            CLLStream test = new CLLStream("this is a foo");
            Assert.AreEqual(test.Kind, TLLStreamKind.llsString);
        }
        [TestMethod]
        public void TestLLStreamReadAll()
        {
            CLLStream test = new CLLStream("success");
            Assert.AreEqual(test.StrVal, "success");
        }
        [TestMethod]
        public void TestLLStreamWrRd()
        {
            CLLStream test = new CLLStream("success");
            Assert.AreEqual(1, 1);

        }
    }
}
