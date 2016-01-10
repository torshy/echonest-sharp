using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EchoNest.Song
{
    public class ProfileArgument
    {
        #region Constructors

        public ProfileArgument()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// TODO: support multiple IDs
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// TODo: support multiple track IDs
        /// </summary>
        public string TrackId { get; set; }

        public SongBucket? Bucket
        {
            get;
            set;
        }

        public bool? Limit
        {
            get;
            set;
        }

        internal string ApiKey
        {
            get;
            set;
        }

        internal string BaseUrl
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            UriQuery query = new UriQuery(BaseUrl);
            query.Add("api_key", ApiKey);
            query.Add("format", "json");

            if (!string.IsNullOrEmpty(Id))
            {
                query.Add("id", Id);
            }

            if (!string.IsNullOrEmpty(TrackId))
            {
                query.Add("track_id", TrackId);
            }

            if (Bucket.HasValue)
            {
                foreach (var bucket in Bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucket);
                }
            }

            if (Limit.HasValue)
            {
                query.Add("limit", Limit.Value.ToString().ToLower());
            }

            return query.ToString();
        }

        #endregion
    }
}
