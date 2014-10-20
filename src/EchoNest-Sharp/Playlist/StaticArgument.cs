using System.Linq;

namespace EchoNest.Playlist
{
    public class StaticArgument : BasicArgument
    {
        #region Constructors

        public StaticArgument()
        {
            Styles = new TermList();
            Moods = new TermList();
            Description = new TermList();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     The artist_pick parameter is used to determine how songs are picked for each artist in artist-type playlists. If the asc or desc suffix is ommitted, artist_pick defaults to descending.
        /// </summary>
        /// <example>
        ///     song_hotttness-desc tempo, duration, loudness, mode, key
        /// </example>
        /// <remarks>
        ///     (default = song_hotttness-desc)
        /// </remarks>
        public string ArtistPick
        {
            get; set;
        }

        /// <summary>
        ///     the maximum variety of artists to be represented in the playlist. A higher number will allow for more variety in the artists.
        /// </summary>
        /// <example>
        ///     0 - 1
        /// </example>
        /// <remarks>
        ///     (default = 0.3)
        /// </remarks>
        public double? Variety
        {
            get; set;
        }

        /// <summary>
        ///     Controls the distribution of artists in the playlist. A focused distribution yields a playlist of songs that are tightly clustered around the seeds, whereas a wandering distribution yields a playlist from a broader range of artists.
        /// </summary>
        /// <example>
        ///     focused, wandering
        /// </example>
        /// <remarks>
        ///     (default = focused)
        /// </remarks>
        public string Distribution
        {
            get; set;
        }

        /// <summary>
        ///     Controls the trade between known music and unknown music. A value of 0 means no adventurousness, only known and preferred music will be played. A value of 1 means high adventurousness, mostly unknown music will be played. A value of auto indicates that the adventurousness should be automatically determined based upon the taste profile of the user. This parameter only applies to catalog and catalog-radio type playlists.
        /// </summary>
        /// <example>
        ///     0 - 1
        /// </example>
        /// <remarks>
        ///     (default = 0.2)
        /// </remarks>
        public double? Adventurousness
        {
            get; set;
        }

        /// <summary>
        ///     ID of seed artist catalog for the playlist
        /// </summary>
        /// <example>
        ///     CAKSMUX1321A708AA4
        /// </example>
        public string SeedCatalog
        {
            get; set;
        }

        /// <summary>
        ///     description of the type of songs that can be included in the playlist
        /// </summary>
        /// <example>
        ///     alt-rock,-emo,harp^2
        /// </example>
        public TermList Description
        {
            get; private set;
        }

        /// <summary>
        ///     a musical style or genre like rock, jazz, or funky. See the method list_terms for details on what styles are currently available
        /// </summary>
        /// <example>
        ///     jazz, metal^2
        /// </example>
        public TermList Styles
        {
            get; private set;
        }

        /// <summary>
        ///     a mood like happy or sad. See the method list_terms for details on what moods are currently available
        /// </summary>
        /// <example>
        ///     happy, sad^.5
        /// </example>
        public TermList Moods
        {
            get; private set;
        }

        /// <summary>
        ///     the minimum artist hotttnesss for songs in the playlist
        /// </summary>
        /// <example>
        ///     0.0 &lt; hotttnesss &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=0.0)
        /// </remarks>
        public double? ArtistMinHotttnesss
        {
            get; set;
        }

        /// <summary>
        ///     the maximum artist hotttness for songs in the playlist
        /// </summary>
        /// <example>
        ///     0.0 &lt; hotttnesss &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=1.0)
        /// </remarks>
        public double? ArtistMaxHotttnesss
        {
            get; set;
        }

        /// <summary>
        ///     the minimum artist familiarity for songs in the playlist
        /// </summary>
        /// <example>
        ///     0.0 &lt; familiarity  &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=0.0)
        /// </remarks>
        public double? ArtistMinFamiliarity
        {
            get; set;
        }

        /// <summary>
        ///     the maximum artist familiarity for songs in the playlist
        /// </summary>
        /// <example>
        ///     0.0 &lt; familiarity  &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=1.0)
        /// </remarks>
        public double? ArtistMaxFamiliarity
        {
            get; set;
        }

        /// <summary>
        ///     Matches artists that have an earliest start year after the given value
        /// </summary>
        /// <example>
        ///     1970, 2011, present
        /// </example>
        public string ArtistStartYearAfter
        {
            get; set;
        }

        /// <summary>
        ///     Matches artists that have an earliest start year before the given value
        /// </summary>
        /// <example>
        ///     1970, 2011, present
        /// </example>
        public string ArtistStartYearBefore
        {
            get; set;
        }

        /// <summary>
        ///     Matches artists that have a latest end year after the given value
        /// </summary>
        /// <example>
        ///     1970, 2011, present
        /// </example>
        public string ArtistEndYearAfter
        {
            get; set;
        }

        /// <summary>
        ///     Matches artists that have a latest end year before the given value
        /// </summary>
        /// <example>
        ///     1970, 2011, present
        /// </example>
        public string ArtistEndYearBefore
        {
            get; set;
        }

        /// <summary>
        ///     the key of songs in the playlist
        /// </summary>
        /// <example>
        ///     (c, c-sharp, d, e-flat, e, f, f-sharp, g, a-flat, a, b-flat, b) 0 - 11
        /// </example>
        public string Key
        {
            get; set;
        }

        /// <summary>
        ///     the minimum danceability of any song
        /// </summary>
        /// <example>
        ///     0.0 &lt; danceability &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=0.0)
        /// </remarks>
        public double? MinDanceability
        {
            get; set;
        }

        /// <summary>
        ///     the maximum danceability of any song
        /// </summary>
        /// <example>
        ///     0.0 &lt; danceability &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=1.0)
        /// </remarks>
        public double? MaxDanceability
        {
            get; set;
        }

        /// <summary>
        ///     the minimum energy of any song
        /// </summary>
        /// <example>
        ///     0.0 &lt; energy &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=0.0)
        /// </remarks>
        public double? MinEnergy
        {
            get; set;
        }

        /// <summary>
        ///     the maximum energy of any song
        /// </summary>
        /// <example>
        ///     0.0 &lt; energy &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=1.0)
        /// </remarks>
        public double? MaxEnergy
        {
            get; set;
        }

        /// <summary>
        ///     the minimum latitude for the location of artists in the playlist
        /// </summary>
        /// <example>
        ///     -90.0 &lt; latitude &lt; 90.0
        /// </example>
        /// <remarks>
        ///     (default=-90.0)
        /// </remarks>
        public double? MinLatitude
        {
            get; set;
        }

        /// <summary>
        ///     the maximum latitude for the location of artists in the playlist
        /// </summary>
        /// <example>
        ///     -90.0 &lt; latitude &lt; 90.0
        /// </example>
        /// <remarks>
        ///     (default=90.0)
        /// </remarks>
        public double? MaxLatitude
        {
            get; set;
        }

        /// <summary>
        ///     the minimum longitude for the location of artists in the playlist
        /// </summary>
        /// <example>
        ///     -180.0 &lt; longitude &lt; 180.0
        /// </example>
        /// <remarks>
        ///     (default=-180.0)
        /// </remarks>
        public double? MinLongitude
        {
            get; set;
        }

        /// <summary>
        ///     the maximum longitude for the location of artists in the playlist
        /// </summary>
        /// <example>
        ///     -180.0 &lt; longitude &lt; 180.0
        /// </example>
        /// <remarks>
        ///     (default=180.0)
        /// </remarks>
        public double? MaxLongitude
        {
            get; set;
        }

        /// <summary>
        ///     the minimum loudness of any song on the playlist
        /// </summary>
        /// <example>
        ///     -100.0 &lt; loudness &lt; 100.0 (dB)
        /// </example>
        /// <remarks>
        ///     (default=-100.0)
        /// </remarks>
        public double? MinLoudness
        {
            get; set;
        }

        /// <summary>
        ///     the maximum loudness of any song on the playlist
        /// </summary>
        /// <example>
        ///     -100.0 &lt; loudness &lt; 100.0 (dB)
        /// </example>
        /// <remarks>
        ///     (default=100.0)
        /// </remarks>
        public double? MaxLoudness
        {
            get; set;
        }

        /// <summary>
        ///     the minimum tempo for any included songs
        /// </summary>
        /// <example>
        ///     0.0 &lt; tempo &lt; 500.0 (BPM)
        /// </example>
        /// <remarks>
        ///     (default=0.0)
        /// </remarks>
        public double? MinTempo
        {
            get; set;
        }

        /// <summary>
        ///     the maximum tempo for any included songs
        /// </summary>
        /// <example>
        ///     0.0 &lt; tempo &lt; 500.0 (BPM)
        /// </example>
        /// <remarks>
        ///     (default=500.0)
        /// </remarks>
        public double? MaxTempo
        {
            get; set;
        }

        /// <summary>
        ///     the mode of songs in the playlist
        /// </summary>
        /// <example>
        ///     (minor, major) 0, 1
        /// </example>
        public string Mode
        {
            get; set;
        }

        /// <summary>
        ///     the minimum hotttnesss for songs in the playlist
        /// </summary>
        /// <example>
        ///     0.0 &lt; hotttnesss &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=0.0)
        /// </remarks>
        public double? SongMinHotttnesss
        {
            get; set;
        }

        /// <summary>
        ///     the maximum hotttnesss for songs in the playlist
        /// </summary>
        /// <example>
        ///     0.0 &lt; hotttnesss &lt; 1.0
        /// </example>
        /// <remarks>
        ///     (default=1.0)
        /// </remarks>
        public double? SongMaxHotttnesss
        {
            get; set;
        }

        /// <summary>
        ///     indicates how the songs should be ordered in the playlist
        /// </summary>
        /// <example>
        ///     tempo-asc, duration-asc, loudness-asc, artist_familiarity-asc, artist_hotttnesss-asc, artist_start_year-asc, artist_start_year-desc, artist_end_year-asc, artist_end_year-desc, song_hotttness-asc, latitude-asc, longitude-asc, mode-asc, key-asc, tempo-desc, duration-desc, loudness-desc, artist_familiarity-desc, artist_hotttnesss-desc, song_hotttnesss-desc, latitude-desc, longitude-desc, mode-desc, key-desc, energy-asc, energy-desc, danceability-asc, danceability-desc
        /// </example>
        public string Sort
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public new string ToString()
        {
            UriQuery query = GetUriQuery();
            return query.ToString();
        }

        protected override UriQuery GetUriQuery()
        {
            UriQuery query = base.GetUriQuery();

            if (!string.IsNullOrEmpty(ArtistPick))
            {
                query.Add("artist_pick", ArtistPick);
            }

            if (Variety.HasValue)
            {
                query.Add("variety", Variety.Value);
            }

            if (!string.IsNullOrEmpty(Distribution))
            {
                query.Add("distribution", Distribution);
            }

            if (Adventurousness.HasValue)
            {
                query.Add("adventurousness", Adventurousness.Value);
            }

            if (!string.IsNullOrEmpty(SeedCatalog))
            {
                query.Add("seed_catalog", SeedCatalog);
            }

            if (Description.Any())
            {
                foreach (Term description in Description)
                {
                    query.Add("description", description);
                }
            }

            if (Styles.Any())
            {
                foreach (Term style in Styles)
                {
                    query.Add("style", style);
                }
            }

            if (Moods.Any())
            {
                foreach (Term mood in Moods)
                {
                    query.Add("mood", mood);
                }
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

            if (!string.IsNullOrEmpty(Sort))
            {
                query.Add("sort", Sort);
            }

            return query;
        }

        #endregion Methods
    }
}
