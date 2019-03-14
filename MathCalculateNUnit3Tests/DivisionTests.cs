using NUnit.Framework;
using MathCalculate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathCalculate.MathCalculateNUnit3Tests
{
    [TestFixture()]
    public class DivisionTests
    {
        [Test()]
        [TestCase(3, 2, ExpectedResult = 1.5)]
        [TestCase(700, 100, ExpectedResult = 7)]
        [TestCase(3, 1, ExpectedResult = 3)]
        [TestCase(3888, 2, ExpectedResult = 1944)]
        public double CalculationTest(double divisor, double dividend)
        {
            return Division.Calculation(divisor, dividend);
        }
    }
}