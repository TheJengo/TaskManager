using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helper.Date
{
    public static class DateHelper
    {
        public static DateTime Now()
        {
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);

            return currentDateTime;
        }


        public static DateTime UnixTimestampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            return unixTime > (DateTime.MaxValue - unixStart).TotalSeconds ? unixStart.AddMilliseconds(unixTime) : unixStart.AddSeconds(unixTime);
        }

        public static DateTime UnixTimestampToDateTime(long unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            return unixTime > (DateTime.MaxValue - unixStart).TotalSeconds ? unixStart.AddMilliseconds(unixTime) : unixStart.AddSeconds(unixTime);
        }
    }
}
