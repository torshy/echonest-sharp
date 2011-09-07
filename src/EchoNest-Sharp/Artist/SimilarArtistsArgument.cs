using System.Collections.Generic;

namespace EchoNest.Artist
{
    public class SimilarArtistsArgument
    {
        #region Constructors

        public SimilarArtistsArgument()
        {
            ID = new List<IdSpace>();
        }

        #endregion Constructors

        #region Properties

        public string ArtistEndYearAfter
        {
            get;
            set;
        }

        public string ArtistEndYearBefore
        {
            get;
            set;
        }

        public string ArtistStartYearAfter
        {
            get;
            set;
        }

        public string ArtistStartYearBefore
        {
            get;
            set;
        }

        public ArtistBucket? Bucket
        {
            get;
            set;
        }

        public List<IdSpace> ID
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

        public int? MinResults
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int? Results
        {
            get;
            set;
        }

        public string SeedCatalog
        {
            get;
            set;
        }

        public int? Start
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

            if (!string.IsNullOrEmpty(Name))
            {
                query.Add("name", Name);
            }

            if (ID != null)
            {
                foreach (var idSpace in ID)
                {
                    query.Add("id", idSpace);
                }
            }

            if (!string.IsNullOrEmpty(SeedCatalog))
            {
                query.Add("seed_catalog", SeedCatalog);
            }

            if (!string.IsNullOrEmpty(ArtistEndYearAfter))
            {
                query.Add("artist_end_year_after", ArtistEndYearAfter);
            }

            if (!string.IsNullOrEmpty(ArtistEndYearBefore))
            {
                query.Add("artist_end_year_before", ArtistEndYearBefore);
            }

            if (!string.IsNullOrEmpty(ArtistStartYearAfter))
            {
                query.Add("artist_start_year_after", ArtistStartYearAfter);
            }

            if (!string.IsNullOrEmpty(ArtistStartYearBefore))
            {
                query.Add("artist_start_year_before", ArtistStartYearBefore);
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

            if (MinResults.HasValue)
            {
                query.Add("min_results", MinResults.Value);
            }

            if (Start.HasValue)
            {
                query.Add("start", Start.Value);
            }

            return query.ToString();
        }

        #endregion Methods
    }
}