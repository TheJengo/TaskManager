using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetWeekOfTheYear(this DateTime dateTime)
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            var weekOfTheYear = cultureInfo.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekOfTheYear;
        }
    }
}
