using System.Threading.Tasks;

namespace EchoNest.Playlist
{
    public class SessionInfo : EchoNestService
    {
        #region Fields

        private const string Url = "playlist/session_info";

        #endregion Fields

        #region Methods

        public SessionInfoResponse Execute(SessionInfoArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<SessionInfoResponse>(argument.ToString());
        }

        public Task<SessionInfoResponse> ExecuteAsync(SessionInfoArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<SessionInfoResponse>(argument.ToString());
        }

        #endregion Methods
    }
}