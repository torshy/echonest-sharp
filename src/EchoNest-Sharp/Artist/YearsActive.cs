using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class YearsActive
    {
        [DataMember(Name = "start")]
        public int Start { get; set; }
        [DataMember(Name = "End")]
        public int? End { get; set; }
    }

    [Flags]
    public enum Bucket
    {
        [Description("biographies")]
        Biographies = 1,
        [Description("blogs")]
        Blogs = 2,
        [Description("doc_counts")]
        DocCounts = 4,
        [Description("familiarity")]
        Familiarity = 8,
        [Description("hotttnesss")]
        Hotttnesss = 16,
        [Description("images")]
        Images = 32,
        [Description("news")]
        News = 64,
        [Description("reviews")]
        Reviews = 128,
        [Description("songs")]
        Songs = 256,
        [Description("terms")]
        Terms = 512,
        [Description("urls")]
        Urls = 1024,
        [Description("video")]
        Video = 2048,
        [Description("audio")]
        Audio = 4096
    }

    public static class BucketExtensions
    {
        public static IEnumerable<Bucket> GetBuckets(this Bucket bucket)
        {
            var buckets = bucket.ToString().Split(',');

            foreach (var s in buckets)
            {
                Bucket parsed;
                if (Enum.TryParse(s.Trim(), out parsed))
                {
                    yield return parsed;
                }
            }
        }

        public static IEnumerable<string> GetBucketDescriptions(this Bucket bucket)
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