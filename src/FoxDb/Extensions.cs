using System;

namespace FoxDb
{
    internal static class Extensions
    {

        public static long AsUnixTimestamp(this DateTime dateTime)
        {
            return (long) dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

    }
}
