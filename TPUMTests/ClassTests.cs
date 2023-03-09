using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPUM;

namespace TPUMTests
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void TestA()
        {
            Class c = new Class();
            c.A();
        }

        [TestMethod]
        public void TestB()
        {
            Class c = new Class();
            Assert.AreEqual(0, c.B());
        }

        [TestMethod]
        public void TestC()
        {
            Class c = new Class();
            Assert.IsFalse(c.C());
        }
    }
}
