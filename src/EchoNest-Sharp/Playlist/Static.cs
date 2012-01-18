using System.Threading.Tasks;

namespace EchoNest.Playlist
{
    public class Static : EchoNestService
    {
        #region Fields

        private const string Url = "playlist/static";

        #endregion Fields

        #region Methods

        public PlaylistResponse Execute(StaticArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<PlaylistResponse>(argument.ToString());
        }

        public Task<PlaylistResponse> ExecuteAsync(StaticArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<PlaylistResponse>(argument.ToString());
        }

        #endregion Methods
    }
}