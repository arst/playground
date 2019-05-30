using System;
using System.Collections.Generic;
using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class HolidaysRuleTests
    {
        private readonly Calculator calculator;

        public static IEnumerable<object[]> Days {
                get {
                    yield return new object[] {false, new DateTime(2016, 1, 1)};
                    yield return new object[] {false, new DateTime(2016, 1, 7)};
                    yield return new object[] {false, new DateTime(2016, 3, 8)};
                    yield return new object[] {false, new DateTime(2016, 5, 1)};
                    yield return new object[] {false, new DateTime(2016, 5, 9)};
                    yield return new object[] {false, new DateTime(2016, 12, 25)};
                }
            }

        public HolidaysRuleTests()
        {
            this.calculator = new Calculator();
            this.calculator.AddRule(new HolidaysRule());
        }

        [Theory]
        [MemberData(nameof(Days))]
        public void TestCheckIsBusinessDay(bool expected, DateTime dateTime)
        {
            Assert.Equal(expected, calculator.IsBussinessDay(dateTime));
        }
    }
}
