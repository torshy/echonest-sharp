using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EchoNest.Artist
{
    public static class ArtistBucketExtensions
    {
        public static IEnumerable<ArtistBucket> GetBuckets(this ArtistBucket bucket)
        {
            var buckets = bucket.ToString().Split(',');

            foreach (var s in buckets)
            {
                ArtistBucket parsed;
                if (Enum.TryParse(s.Trim(), out parsed))
                {
                    yield return parsed;
                }
            }
        }

        public static IEnumerable<string> GetBucketDescriptions(this ArtistBucket bucket)
        {
            var buckets = bucket.GetBuckets();

            foreach (var b in buckets)
            {
                yield return EnumHelpers.GetDescription(b);
            }
        }
    }

    internal class EnumHelpers
    {
        internal static string GetDescription(Enum value)
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