namespace EchoNest.Artist
{
    public class ExtractArgument
    {
        #region Properties

        public ArtistBucket? Bucket
        {
            get;
            set;
        }

        public bool? Limit
        {
            get;
            set;
        }

        public double? MaxFamiliarity
        {
            get;
            set;
        }

        public double? MaxHotttnesss
        {
            get;
            set;
        }

        public double? MinFamiliarity
        {
            get;
            set;
        }

        public double? MinHotttnesss
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public int? Results
        {
            get;
            set;
        }

        public Sorting? Sort
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

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            UriQuery query = new UriQuery(BaseUrl);
            query.Add("api_key", ApiKey);
            query.Add("format", "json");

            if (!string.IsNullOrEmpty(Text))
            {
                query.Add("text", Text);
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
                query.Add("limit", Limit.Value);
            }

            if (MaxFamiliarity.HasValue)
            {
                query.Add("max_familiarity", MaxFamiliarity.Value);
            }

            if (MinFamiliarity.HasValue)
            {
                query.Add("min_familiarity", MinFamiliarity.Value);
            }

            if (MaxHotttnesss.HasValue)
            {
                query.Add("max_hotttnesss", MaxHotttnesss.Value);
            }

            if (MinHotttnesss.HasValue)
            {
                query.Add("min_hotttnesss", MinHotttnesss.Value);
            }

            if (Results.HasValue)
            {
                query.Add("results", Results.Value);
            }

            if (Sort.HasValue)
            {
                query.Add("sort", EnumHelpers.GetDescription(Sort.Value));
            }

            return query.ToString();
        }

        #endregion Methods
    }
}