using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL.V1.Extensions
{
    public static class DateTimeExtensions
    { 
        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }

        public static int weekProjector(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
               dateTime,
               CalendarWeekRule.FirstFourDayWeek,
               DayOfWeek.Sunday);
        }
    }
}
