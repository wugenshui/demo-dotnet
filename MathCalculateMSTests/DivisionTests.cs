using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathCalculate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathCalculate.MathCalculateNUnit3Tests
{
    [TestClass()]
    public class DivisionTests
    {
        [TestMethod()]
        public void CalculationTest_Return5()
        {
            Assert.AreEqual(5, Division.Calculation(5, 1));

            Assert.AreEqual(2.5, Division.Calculation(5, 2));

            Assert.AreEqual(2, Division.Calculation(4, 2));

            Assert.AreEqual(5, Division.Calculation(5, 1));

            Assert.AreEqual(5, Division.Calculation(5, 1));
        }

        //[DataTestMethod]
        //[DataRow(700, 100, 7)]
        //[DataRow(3, 2, 1.5)]
        //[DataRow(0, 2, 0)]
        //[DataRow(-3, 1, -3)]
        //public void AdditionTest(int a, int b, int result)
        //{
        //    Assert.AreEqual(result, a + b);
        //}
    }
}