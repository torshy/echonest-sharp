namespace EchoNest.Playlist
{
    /// <summary>
    ///     Returns state information for dynamic playlists.
    /// </summary>
    public class SessionInfoArgument
    {
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

        #endregion Properties

        #region Methods

        public new string ToString()
        {
            UriQuery query = GetUriQuery();
            return query.ToString();
        }

        protected UriQuery GetUriQuery()
        {
            UriQuery query = new UriQuery(BaseUrl);
            query.Add("api_key", ApiKey);
            query.Add("format", "json");

            if (!string.IsNullOrEmpty(SessionId))
            {
                query.Add("session_id", SessionId);
            }

            return query;
        }

        #endregion Methods
    }
}