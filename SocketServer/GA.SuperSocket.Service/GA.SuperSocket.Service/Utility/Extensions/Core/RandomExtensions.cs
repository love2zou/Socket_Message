using System;

namespace Globalegrow.Toolkit
{
    public static class RandomExtensions
    {
        public static bool NextBoolean(this Random random)
        {
            return random.Next(byte.MinValue, byte.MaxValue) > (byte.MaxValue / 2);
        }

        public static DateTime NextDateTime(this Random random)
        {
            return NextDateTime(random, DateTime.MinValue.Year, DateTime.MaxValue.Year);
        }

        public static DateTime NextDateTime(this Random random, int minYear, int maxYear)
        {
            int year = random.Next(minYear, maxYear);
            int month = random.Next(1, 12);
            int day = random.Next(1, 28);
            int hour = random.Next(0, 23);
            int minute = random.Next(0, 59);
            int second = random.Next(0, 59);

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}