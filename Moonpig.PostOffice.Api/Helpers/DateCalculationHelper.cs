using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moonpig.PostOffice.Api.Helpers
{
    public class DateCalculationHelper
    {
        public static DateTime EnsureDateIsWeekDay(DateTime dt)
        {
            var daysToAdd = 0;

            if (dt.DayOfWeek == DayOfWeek.Saturday)
                daysToAdd = 2;
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
                daysToAdd = 1;

            return dt.AddDays(daysToAdd);
        }

        public static DateTime AddBusinessDaysToDate(DateTime dt, int numOfDays)
        {
            for (var i = 0; i < numOfDays; i++)
            {
                //Add 1 day to date, being sure to skip weekend days
                dt = EnsureDateIsWeekDay(dt.AddDays(1));
            }

            return dt;
        }

    }
}
