using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EchoNest.Track
{
    public class ProfileArgument
    {
        #region Constructors

        public ProfileArgument()
        {
        }

        #endregion Constructors

        #region Properties

        public string Id { get; set; }

        public string Md5 { get; set; }

        public TrackBucket? Bucket
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

            if (!string.IsNullOrEmpty(Md5))
            {
                query.Add("md5", Md5);
            }
            
            if (Bucket.HasValue)
            {
                foreach (var bucket in Bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucket);
                }
            }

            return query.ToString();
        }
        #endregion
    }
}
