using System;

namespace EchoNest.Playlist
{
    /// <summary>
    ///     When a dynamic playlist request is made, a single song is returned along with a session ID. To get the next song in the playlist, a dynamic request with the session ID is made. After creating the initial playlist, the only parameters necessary to fetch songs are the api key, session ID, and format. If parameters like artist, artist_id, song, or description are provided, the api will interpret this as a request to create a new playlist on the same session. Some parameters are 'steering' parameters and will adjust the current playlist as opposed to resetting it. The parameters that you can use within an existing session to steer a playlist without resetting the sesion are:
    ///     <list type = "bullet">
    ///         <item>session_id</item>
    ///         <item>rating</item>
    ///         <item>steer</item>
    ///         <item>steer_description</item>
    ///         <item>steer_style</item>
    ///         <item>steer_mood</item>
    ///         <item>ban</item>
    ///         <item>chain_xspf</item>
    ///     </list>
    /// </summary>
    public class DynamicArgument : StaticArgument
    {
        #region Properties

        /// <summary>
        /// Number of results to return is not applicable to dynamic playlist because dynamic playlist returns only 1 result by design
        /// </summary>
        public override int? Results
        {
            get { return null; }
            set { throw new ArgumentException("Results. Number of results to return is not applicable to dynamic playlist because dynamic playlist returns only 1 result by design"); }
        }

        /// <summary>
        ///     The id of the current playlist session. To start a new session, call playlist/dynamic with no session ID.
        /// </summary>
        /// <remarks>
        ///     required = no. Set value to the id return by previous playlist/dynamic call
        /// </remarks>
        /// <example>
        ///     c1fdacd5a1164449b49584398ca807f3
        /// </example>
        public string SessionId
        {
            get; set;
        }

        /// <summary>
        ///     The user rating for the previous track. 5 is the highest rating, 1 is the lowest rating. Some playlist algorithms will adapt playlists based upon these ratings.
        /// </summary>
        /// <example>
        ///     1,2,3,4 or 5
        /// </example>
        /// <remarks>
        ///     required = no
        /// </remarks>
        public int? Rating
        {
            get; set;
        }

        /// <summary>
        ///     If true, an additional 'track' is added to the end XSPF playlist that is a URL to the next XSPF in the chain.
        /// </summary>
        /// <example>
        ///     true or false
        /// </example>
        /// <remarks>
        ///     required = no
        ///     default = false
        /// </remarks>
        public bool? ChainXspf
        {
            get; set;
        }

        /// <summary>
        ///     Using the previously played track as the basis, make all future tracks some multiplier of the selected attribute(s). required terms use a multiplier in a boost-like format - e.g. energy^.5 to make all future songs have half the energy of the previously played song, or loudness^1.3 to add 30%.
        /// </summary>
        /// <example>
        ///     tempo, loudness, danceability, energy, song_hotttnesss, artist_hotttnesss, artist_familiarity (plus multiplier, see description)
        /// </example>
        /// <remarks>
        ///     required = no, multiple = yes
        /// </remarks>
        public string Steer
        {
            get; set;
        }

        /// <summary>
        ///     Using the current playlist as a basis, add or remove terms relating to the artists and songs to be played. Applying a term again will overwrite the previous term, including boost. Boosting with a "0" will remove a term from the list of qualifiers. Using 'description' rather than 'steer_description' will clear the description list and restart the playlist session with the new term.
        /// </summary>
        /// <example>
        ///     alt-rock, -emo, harp^2, country^0
        /// </example>
        /// <remarks>
        ///     required = no, multiple = yes
        /// </remarks>
        public string SteerDescription
        {
            get; set;
        }

        /// <summary>
        ///     Using the current playlist as a basis, add or remove terms relating to the style of artists and songs to be played. Applying a style again will overwrite the previous style, including boost. Boosting with a "0" will remove a term from the list of qualifiers. Using 'style' rather than 'steer_style' will clear the style list and restart the playlist session with the new style.
        /// </summary>
        /// <example>
        ///     alt-rock, -emo, harp^2, country^0
        /// </example>
        /// <remarks>
        ///     required = no, multiple = yes
        /// </remarks>
        public string SteerStyle
        {
            get; set;
        }

        /// <summary>
        ///     Using the current playlist as a basis, add or remove terms relating to the mood of artists and songs to be played. Applying a mood again will overwrite the previous mood, including boost. Boosting with a "0" will remove a mood from the list of moods. Using 'mood' rather than 'steer_mood' will clear the mood list and restart the playlist session with the new mood.
        /// </summary>
        /// <example>
        ///     happy, -sad, angry^2, chill^0
        /// </example>
        /// <remarks>
        ///     required = no, multiple = yes
        /// </remarks>
        public string SteerMood
        {
            get; set;
        }

        /// <summary>
        ///     Banning will prevent the song/all songs by that artist from appearing again in the current playlist session.
        /// </summary>
        /// <example>
        ///     song, artist
        /// </example>
        /// <remarks>
        ///     required = no, multiple = yes
        /// </remarks>
        public string Ban
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

            if (!string.IsNullOrEmpty(SessionId))
            {
                query.Add("session_id", SessionId);
            }

            if (Rating.HasValue)
            {
                query.Add("rating", Rating);
            }

            if (ChainXspf.HasValue)
            {
                query.Add("chain_xspf", ChainXspf);
            }

            if (!string.IsNullOrEmpty(Steer))
            {
                query.Add("steer", Steer);
            }

            if (!string.IsNullOrEmpty(SteerDescription))
            {
                query.Add("steer_description", SteerDescription);
            }

            if (!string.IsNullOrEmpty(SteerStyle))
            {
                query.Add("steer_style", SteerStyle);
            }

            if (!string.IsNullOrEmpty(SteerMood))
            {
                query.Add("steer_mood", SteerMood);
            }

            if (!string.IsNullOrEmpty(Ban))
            {
                query.Add("ban", Ban);
            }

            return query;
        }

        #endregion Methods
    }
}