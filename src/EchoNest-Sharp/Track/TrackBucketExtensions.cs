using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EchoNest.Track
{
    public static class TrackBucketExtensions
    {
        public static IEnumerable<TrackBucket> GetBuckets(this TrackBucket bucket)
        {
            var buckets = bucket.ToString().Split(',');

            foreach (var s in buckets)
            {
                TrackBucket parsed;
                if (Enum.TryParse(s.Trim(), out parsed))
                {
                    yield return parsed;
                }
            }
        }

        public static IEnumerable<string> GetBucketDescriptions(this TrackBucket bucket)
        {
            var buckets = bucket.GetBuckets();

            foreach (var b in buckets)
            {
                yield return GetDescription(b);
            }
        }

        private static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }
    }
}
