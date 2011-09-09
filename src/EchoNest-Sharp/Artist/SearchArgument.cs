using System.Linq;

namespace EchoNest.Artist
{
    public class SearchArgument
    {
        #region Constructors

        public SearchArgument()
        {
            Styles = new TermList();
            Moods = new TermList();
            Description = new TermList();
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

        public TermList Description
        {
            get;
            private set;
        }

        public bool? FuzzyMatch
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

        public TermList Moods
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }

        public RankType? RankType
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

        public int? Start
        {
            get;
            set;
        }

        public TermList Styles
        {
            get;
            private set;
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

            if (Styles.Count() > 0)
            {
                foreach (var style in Styles)
                {
                    query.Add("style", style);
                }
            }

            if (Moods.Count() > 0)
            {
                foreach (var mood in Moods)
                {
                    query.Add("mood", mood);
                }
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

            if (Description.Count() > 0)
            {
                foreach (var description in Description)
                {
                    query.Add("description", description);
                }
            }

            if (FuzzyMatch.HasValue)
            {
                query.Add("fuzzy_match", FuzzyMatch.Value);
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

            if (RankType.HasValue)
            {
                query.Add("rank_type", RankType.Value.ToString().ToLower());
            }

            if (Results.HasValue)
            {
                query.Add("results", Results.Value);
            }

            if (Start.HasValue)
            {
                query.Add("start", Start.Value);
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