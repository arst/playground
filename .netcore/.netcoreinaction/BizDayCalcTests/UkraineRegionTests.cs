using System;
using System.Collections.Generic;
using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class UkraineRegionTests : IClassFixture<UkraineRegionFixture>
    {
        private readonly UkraineRegionFixture fixture;

        public UkraineRegionTests(UkraineRegionFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData("2016-01-01")]
        [InlineData("2016-12-25")]
        [Trait("Holidays", "true")]
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
