using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EchoNest.Playlist
{
    public static class PlaylistBucketExtensions
    {
        public static IEnumerable<PlaylistBucket> GetBuckets(this PlaylistBucket bucket)
        {
            string[] buckets = bucket.ToString().Split(',');

            foreach (string s in buckets)
            {
                PlaylistBucket parsed;
                if (Enum.TryParse(s.Trim(), out parsed))
                {
                    yield return parsed;
                }
            }
        }

        public static IEnumerable<string> GetBucketDescriptions(this PlaylistBucket bucket)
        {
            IEnumerable<PlaylistBucket> buckets = bucket.GetBuckets();

            foreach (PlaylistBucket b in buckets)
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
