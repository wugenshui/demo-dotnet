using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathCalculate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCalculate.MSTests
{
    [TestClass()]
    public class DivisionTests
    {
        [DataTestMethod]
        [DataRow(700, 100, 7)]
        [DataRow(3, 2, 1.5)]
        [DataRow(0, 2, 0)]
        [DataRow(-3, 1, -3)]
        public void CalculationTest(double divisor, double dividend, double expectedResult)
        {
            double actualResult = Division.Calculation(divisor, dividend);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void CalculationTest_DivideZero_ThrowException()
        {
            Assert.ThrowsException<DivideByZeroException>(delegate ()
            {
                Division.Calculation(12, 0);
            });
        }
    }
}