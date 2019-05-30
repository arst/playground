using System;
using System.Collections.Generic;
using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class WeekendRuleTests
    {
        public static IEnumerable<object[]> Days {
                get {
                    yield return new object[] {true,  new DateTime(2016,  6, 27)};
                    yield return new object[] {true,  new DateTime(2016,  3,  1)};
                    yield return new object[] {false, new DateTime(2016,  6, 26)};
                    yield return new object[] {false, new DateTime(2016, 11, 12)};
                }
            }

        [Theory]
        [InlineData("2016-06-27")] // Monday                     
        [InlineData("2016-03-01")] // Tuesday
        [InlineData("2017-09-20")] // Wednesday
        public void IsBusinessDay(string date)              
        {
            var rule = new WeekendRule();
            Assert.True(rule.CheckIsBussinessDay(DateTime.Parse(date)));
        }

        [Theory]
        [InlineData("2016-06-26")]
        [InlineData("2016-11-12")]
        public void IsNotBusinessDay(string date)
        {
            var rule = new WeekendRule();
            Assert.False(rule.CheckIsBussinessDay(DateTime.Parse(date)));
        }

        [Theory]
        [MemberData(nameof(Days))]
        public void TestCheckIsBusinessDay(bool expected, DateTime dateTime)
        {
            var rule = new WeekendRule();
            Assert.Equal(expected, rule.CheckIsBussinessDay(dateTime));
        }
    }
}
