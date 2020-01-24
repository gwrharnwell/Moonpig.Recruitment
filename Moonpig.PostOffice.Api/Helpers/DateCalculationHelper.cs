using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moonpig.PostOffice.Api.Helpers
{
    public class DateCalculationHelper
    {
        /// <summary>
        /// Takes any datetime and returns it as is if it's a weekday or returns the following Monday if it's a weekend day.
        /// </summary>
        /// <param name="dt">A DateTime value</param>
        /// <returns>Valid week day datetime</returns>
        public static DateTime EnsureDateIsWeekDay(DateTime dt)
        {
            var daysToAdd = 0;

            if (dt.DayOfWeek == DayOfWeek.Saturday)
                daysToAdd = 2;
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
                daysToAdd = 1;

            return dt.AddDays(daysToAdd);
        }

        /// <summary>
        /// Adds days to any date, excluding weekends
        /// </summary>
        /// <param name="dt">The start date</param>
        /// <param name="numOfDays">Number of days to add to start date</param>
        /// <returns>Calculated end date</returns>
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
