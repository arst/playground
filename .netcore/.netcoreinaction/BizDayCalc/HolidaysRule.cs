using System;

namespace BizDayCalc
{
    public class HolidaysRule : IRule
    {

        private static readonly int [,] UkraineHolidays = {
            { 1, 1 },
            { 7, 1 },
            { 8, 3 },
            { 1, 5 },
            { 9, 5 },
            { 25, 12 }
        };

         public bool CheckIsBussinessDay(DateTime dateTime)
         {
             for (int i = 0; i <=  UkraineHolidays.GetUpperBound(0); i++)
             {
                 if (dateTime.Day == UkraineHolidays[i, 0] && dateTime.Month == UkraineHolidays[i, 1])
                 {
                     return false;
                 }
             }

             return true;
         }
    }
}