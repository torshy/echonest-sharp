using System.Threading.Tasks;

namespace EchoNest.Song
{
    public class Search : EchoNestService
    {
        #region Fields

        private const string Url = "song/search";

        #endregion Fields

        #region Methods

        public SearchResponse Execute(SearchArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<SearchResponse>(argument.ToString());
        }

        public Task<SearchResponse> ExecuteAsync(SearchArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<SearchResponse>(argument.ToString());
        }

        #endregion Methods
    }
}