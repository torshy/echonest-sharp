using System.Linq;

namespace EchoNest.Playlist
{
    public class BasicArgument
    {
        #region Constructors

        public BasicArgument()
        {
            ArtistID = new TermList();
            Artist = new TermList();
            SongID = new TermList();
            TrackID = new TermList();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     The API url
        /// </summary>
        public string BaseUrl
        {
            get; set;
        }

        /// <summary>
        ///     your API key
        /// </summary>
        public string ApiKey
        {
            get; set;
        }

        /// <summary>
        ///     the type of the playlist to be generated.
        /// </summary>
        /// <example>
        ///     <list type = "bullet">
        ///         <item>artist - plays songs for the given artists</item>
        ///         <item>artist-radio - plays songs for the given artists and similar artists</item>
        ///         <item>artist-description - plays songs from artists matching the given description</item>
        ///         <item>song-radio - plays songs similar to the song specified.</item>
        ///         <item>catalog - the playlist is personalized based upon the given seed catalog. Results are limited to items listed in the given catalog.</item>
        ///         <item>catalog-radio - the playlist is personalized based upon the given seed catalog. Results are limited to items listed in the given catalog and items that are similar to items in the given catalog.</item>
        ///     </list>
        /// </example>
        public string Type
        {
            get; set;
        }

        /// <summary>
        ///     Multiples allowed (no more than 5 total total artist_id, artist, track_id, and song_id parameters)
        ///     ID(s) of seed artist(s) for the playlist. Echo Nest or Rosetta IDs (See Project Rosetta Stone)
        /// </summary>
        /// <example>
        ///     ARH6W4X1187B99274F, 7digital-US:artist:304
        /// </example>
        public TermList ArtistID
        {
            get; set;
        }

        /// <summary>
        ///     Name(s) of seed artist(s) for the playlist
        ///     Multiples allowed (no more than 5 total total artist_id, artist, track_id, and song_id parameters)
        /// </summary>
        /// <example>
        ///     Weezer, the+beatles
        /// </example>
        public TermList Artist
        {
            get; set;
        }

        /// <summary>
        ///     ID(s) of seed song(s) for the playlist (used by some types). Echo Nest or Rosetta IDs (See Project Rosetta Stone)
        ///     Multiples allowed (no more than 5 total total artist_id, artist, track_id, and song_id parameters)
        /// </summary>
        /// <example>
        ///     SOCZMFK12AC468668F
        /// </example>
        public TermList SongID
        {
            get; set;
        }

        /// <summary>
        ///     ID(s) of seed tracks(s) for the playlist (used by playlist types that accept songs as seeds) Echo Nest or Rosetta IDs (See Project Rosetta Stone)
        ///     Multiples allowed (no more than 5 total total artist_id, artist, track_id, and song_id parameters)
        /// </summary>
        /// <example>
        ///     TRTLKZV12E5AC92E11
        /// </example>
        public TermList TrackID
        {
            get; set;
        }

        /// <summary>
        ///     The desired number of songs in the playlist
        /// </summary>
        /// <example>
        ///     0 - 100
        /// </example>
        /// <remarks>
        ///     (default = 15)
        /// </remarks>
        public virtual int? Results
        {
            get; set;
        }

        /// <summary>
        ///     Specifies which rosetta id space info should be returned
        /// </summary>
        /// <example>
        ///     id:catalog-name, tracks
        /// </example>
        public PlaylistBucket? Bucket
        {
            get; set;
        }

        /// <summary>
        ///     If 'true', limit the results to any of the given rosetta id space
        /// </summary>
        /// <remarks>
        ///     (default = false)
        /// </remarks>
        public bool? Limit
        {
            get; set;
        }

        /// <summary>
        ///     If true the playlist delivered will meet the DMCA rules (see below).
        /// </summary>
        /// <remarks>
        ///     (default = false)
        /// </remarks>
        public bool? Dmca
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            UriQuery query = GetUriQuery();
            return query.ToString();
        }

        protected virtual UriQuery GetUriQuery()
        {
            UriQuery query = new UriQuery(BaseUrl);
            query.Add("api_key", ApiKey);
            query.Add("format", "json");

            if (!string.IsNullOrEmpty(Type))
            {
                query.Add("type", Type);
            }

            if (ArtistID.Count() > 0)
            {
                foreach (Term artistId in ArtistID)
                {
                    query.Add("artist_id", artistId);
                }
            }

            if (Artist.Count() > 0)
            {
                foreach (Term artist in Artist)
                {
                    query.Add("artist", artist);
                }
            }

            if (SongID.Count() > 0)
            {
                foreach (Term songId in SongID)
                {
                    query.Add("song_id", songId);
                }
            }

            if (TrackID.Count() > 0)
            {
                foreach (Term trackId in TrackID)
                {
                    query.Add("track_id", trackId);
                }
            }

            if (Results.HasValue)
            {
                query.Add("results", Results.Value);
            }

            if (Bucket.HasValue)
            {
                foreach (string bucket in Bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucket);
                }
            }

            if (Limit.HasValue)
            {
                query.Add("limit", Limit.Value.ToString().ToLower());
            }

            if (Dmca.HasValue)
            {
                query.Add("dmca", Dmca.Value.ToString().ToLower());
            }

            return query;
        }

        #endregion Methods
    }
}
