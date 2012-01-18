using System.Threading.Tasks;

namespace EchoNest.Playlist
{
    public class Basic : EchoNestService
    {
        #region Fields

        private const string Url = "playlist/basic";

        #endregion Fields

        #region Methods

        public PlaylistResponse Execute(BasicArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<PlaylistResponse>(argument.ToString());
        }

        public Task<PlaylistResponse> ExecuteAsync(BasicArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<PlaylistResponse>(argument.ToString());
        }

        #endregion Methods
    }
}