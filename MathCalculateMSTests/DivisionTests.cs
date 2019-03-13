using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathCalculate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathCalculate.MSTests
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
    }
}