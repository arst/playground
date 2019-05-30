using BizDayCalc;
using Xunit;

namespace BizDayCalcTests 
{
    public class UkraineRegionFixture 
    {
        public Calculator Calculator { get; private set; }

        public UkraineRegionFixture()
        {
            this.Calculator = new Calculator();
            Calculator.AddRule(new WeekendRule());
            Calculator.AddRule(new HolidaysRule());
        }
    }

    [CollectionDefinition("Ukraine region collection")]
    public class USRegionCollection : ICollectionFixture<UkraineRegionFixture>
    {
    }
}