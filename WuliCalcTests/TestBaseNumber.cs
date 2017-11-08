using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuliCalc;

namespace WuliCalcTests
{
    [TestClass]
    public class TestBaseNumber
    {
        [TestMethod]
        public void TestAdd()
        {
            int max = 1000;
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            for(int i = 0; i < max; i++)
            {
                int i1 = ra.Next();
                int i2 = ra.Next();
                int i3 = ra.Next();
                int i4 = ra.Next();
                UInt64 n1 = (UInt64)(i1 * i2);
                UInt64 n2 = (UInt64)(i3 * i4);
                DoTestAdd(n1, n2);
            }
        }

        public void DoTestAdd(UInt64 n1, UInt64 n2)
        {
            BaseNumber b1 = new BaseNumber(n1);
            BaseNumber b2 = new BaseNumber(n2);
            BaseNumber res = b1 + b2;
            Assert.AreEqual(res.GetData(), (UInt64)(n1 + n2));
        }
    }
}
