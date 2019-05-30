using System;
using System.Collections.Generic;

namespace BizDayCalc
{
    public class Calculator 
    {
        private List<IRule> rules = new List<IRule>();

        public void AddRule(IRule rule)
        {
            rules.Add(rule);
        }

        public bool IsBussinessDay(DateTime dateTime)
        {
            foreach (var rule in rules)
            {
                if (!rule.CheckIsBussinessDay(dateTime))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
