using System;
using System.Collections.Generic;
using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class UkraineRegionTests : IClassFixture<UkraineRegionFixture>
    {
        private readonly UkraineRegionFixture fixture;

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
        public UkraineRegionTests(UkraineRegionFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData("2016-01-01")]
        [InlineData("2016-12-25")]
        public void TestHolidays(string date)
        {
            Assert.False(fixture.Calculator.IsBussinessDay(
            DateTime.Parse(date)));
        }

        [Theory]
        [InlineData("2016-02-29")]
        [InlineData("2016-01-04")]
        public void TestNonHolidays(string date)
        {
            Assert.True(fixture.Calculator.IsBussinessDay(
            DateTime.Parse(date)));
        }
    }
}
