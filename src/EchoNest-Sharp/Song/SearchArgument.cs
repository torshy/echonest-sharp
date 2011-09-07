namespace EchoNest.Song
{
    public class SearchArgument
    {
        #region Properties

        public string Artist
        {
            get;
            set;
        }

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

        public string ArtistID
        {
            get;
            set;
        }

        public int? ArtistMaxFamiliarity
        {
            get;
            set;
        }

        public int? ArtistMaxHotttnesss
        {
            get;
            set;
        }

        public int? ArtistMinFamiliarity
        {
            get;
            set;
        }

        public int? ArtistMinHotttnesss
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

        public SongBucket? Bucket
        {
            get;
            set;
        }

        public string Combined
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public bool? Limit
        {
            get;
            set;
        }

        public int? MaxDanceability
        {
            get;
            set;
        }

        public int? MaxEnergy
        {
            get;
            set;
        }

        public int? MaxLatitude
        {
            get;
            set;
        }

        public int? MaxLongitude
        {
            get;
            set;
        }

        public int? MaxLoudness
        {
            get;
            set;
        }

        public int? MaxTempo
        {
            get;
            set;
        }

        public int? MinDanceability
        {
            get;
            set;
        }

        public int? MinEnergy
        {
            get;
            set;
        }

        public int? MinLatitude
        {
            get;
            set;
        }

        public int? MinLongitude
        {
            get;
            set;
        }

        public int? MinLoudness
        {
            get;
            set;
        }

        public int? MinTempo
        {
            get;
            set;
        }

        public string Mode
        {
            get;
            set;
        }

        public string Mood
        {
            get;
            set;
        }

        public string RankType
        {
            get;
            set;
        }

        public int? Results
        {
            get;
            set;
        }

        public int? SongMaxHotttnesss
        {
            get;
            set;
        }

        public int? SongMinHotttnesss
        {
            get;
            set;
        }

        public string Sort
        {
            get;
            set;
        }

        public int? Start
        {
            get;
            set;
        }

        public string Style
        {
            get;
            set;
        }

        public string Title
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

            if (!string.IsNullOrEmpty(Title))
            {
                query.Add("title", Title);
            }

            if (!string.IsNullOrEmpty(Artist))
            {
                query.Add("artist", Artist);
            }

            if (!string.IsNullOrEmpty(Combined))
            {
                query.Add("combined", Combined);
            }

            if (!string.IsNullOrEmpty(Description))
            {
                query.Add("description", Description);
            }

            if (!string.IsNullOrEmpty(Style))
            {
                query.Add("style", Style);
            }

            if (!string.IsNullOrEmpty(Mood))
            {
                query.Add("mood", Mood);
            }

            if (!string.IsNullOrEmpty(RankType))
            {
                query.Add("rank_type", RankType);
            }

            if (!string.IsNullOrEmpty(ArtistID))
            {
                query.Add("artist_id", ArtistID);
            }

            if (Results.HasValue)
            {
                query.Add("results", Results.Value);
            }

            if (Start.HasValue)
            {
                query.Add("start", Start.Value);
            }

            if (MaxTempo.HasValue)
            {
                query.Add("max_tempo", MaxTempo.Value);
            }

            if (MinTempo.HasValue)
            {
                query.Add("min_tempo", MinTempo.Value);
            }

            if (MaxLoudness.HasValue)
            {
                query.Add("max_loudness", MaxLoudness.Value);
            }

            if (MinLoudness.HasValue)
            {
                query.Add("min_loudness", MinLoudness.Value);
            }

            if (ArtistMaxFamiliarity.HasValue)
            {
                query.Add("artist_max_familiarity", ArtistMaxFamiliarity.Value);
            }

            if (ArtistMinFamiliarity.HasValue)
            {
                query.Add("artist_min_familiarity", ArtistMinFamiliarity.Value);
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

            if (SongMaxHotttnesss.HasValue)
            {
                query.Add("song_max_hotttnesss", SongMaxHotttnesss.Value);
            }

            if (SongMinHotttnesss.HasValue)
            {
                query.Add("song_min_hotttnesss", SongMinHotttnesss.Value);
            }

            if (ArtistMaxHotttnesss.HasValue)
            {
                query.Add("artist_max_hotttnesss", ArtistMaxHotttnesss.Value);
            }

            if (ArtistMinHotttnesss.HasValue)
            {
                query.Add("artist_min_hotttnesss", ArtistMinHotttnesss.Value);
            }

            if (MaxLongitude.HasValue)
            {
                query.Add("max_longitude", MaxLongitude.Value);
            }

            if (MinLongitude.HasValue)
            {
                query.Add("min_longitude", MinLongitude.Value);
            }

            if (MaxLatitude.HasValue)
            {
                query.Add("max_latitude", MaxLatitude.Value);
            }

            if (MinLatitude.HasValue)
            {
                query.Add("min_latitude", MinLatitude.Value);
            }

            if (MaxDanceability.HasValue)
            {
                query.Add("max_danceability", MaxDanceability.Value);
            }

            if (MinDanceability.HasValue)
            {
                query.Add("min_danceability", MinDanceability.Value);
            }

            if (MaxEnergy.HasValue)
            {
                query.Add("max_energy", MaxEnergy.Value);
            }

            if (MinEnergy.HasValue)
            {
                query.Add("min_energy", MinEnergy.Value);
            }

            if (!string.IsNullOrEmpty(Mode))
            {
                query.Add("mode", Mode);
            }

            if (!string.IsNullOrEmpty(Key))
            {
                query.Add("key", Key);
            }

            if (Bucket.HasValue)
            {
                foreach (var bucket in Bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucket);
                }
            }

            if (!string.IsNullOrEmpty(Sort))
            {
                query.Add("sort", Sort);
            }

            if (Limit.HasValue)
            {
                query.Add("limit", Limit.Value.ToString().ToLower());
            }

            return query.ToString();
        }

        #endregion Methods
    }
}