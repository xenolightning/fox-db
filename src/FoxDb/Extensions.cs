using System;
using System.IO;
using Newtonsoft.Json;

namespace FoxDb
{
    internal static class Extensions
    {

        public static long AsUnixTimestamp(this DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }


        public static T Clone<T>(this T item)
        {
            var js = new JsonSerializer
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            using (var jw = new JsonTextWriter(sw))
            {
                js.Serialize(jw, item, typeof(T));

                using (var sr = new StreamReader(ms))
                using (var jr = new JsonTextReader(sr))
                {
                    return js.Deserialize<T>(jr);
                }

            }
        }

    }
}
