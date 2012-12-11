using System.Threading.Tasks;
using System;

namespace EchoNest.Playlist
{
    [Obsolete("Deprecated by Echonest, will be disabled on December 21, 2012", true)]
    public class Dynamic : EchoNestService
    {
        #region Fields

        private const string Url = "playlist/dynamic";

        #endregion Fields

        #region Methods

        public DynamicPlaylistResponse Execute(DynamicArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<DynamicPlaylistResponse>(argument.ToString());
        }

        public Task<DynamicPlaylistResponse> ExecuteAsync(DynamicArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<DynamicPlaylistResponse>(argument.ToString());
        }

        #endregion Methods
    }
}