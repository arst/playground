using System;

namespace BizDayCalc
{
    public class WeekendRule : IRule
    {
         public bool CheckIsBussinessDay(DateTime dateTime)
         {
             return dateTime.DayOfWeek != DayOfWeek.Saturday &&
                dateTime.DayOfWeek != DayOfWeek.Sunday;
         }
    }
}