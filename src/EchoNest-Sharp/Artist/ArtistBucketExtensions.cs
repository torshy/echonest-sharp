using System;
using System.Collections.Generic;

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
}