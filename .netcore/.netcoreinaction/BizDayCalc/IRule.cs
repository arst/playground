using System;

namespace BizDayCalc 
{
    public interface IRule
    {
        bool CheckIsBussinessDay(DateTime dateTime);
    }
}