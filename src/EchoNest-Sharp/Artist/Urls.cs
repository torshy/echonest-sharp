using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Urls : EchoNestService
    {
        #region Fields

        private const string Url = "artist/urls";

        #endregion Fields

        #region Methods

        public UrlsResponse Execute(IdSpace id)
        {
            UriQuery query = GetQuery().Add("id", id);

            return Execute<UrlsResponse>(query.ToString());
        }

        public UrlsResponse Execute(string name)
        {
            UriQuery query = GetQuery().Add("name", name);

            return Execute<UrlsResponse>(query.ToString());
        }

        public Task<UrlsResponse> ExecuteAsync(IdSpace id)
        {
            UriQuery query = GetQuery().Add("id", id);

            return ExecuteAsync<UrlsResponse>(query.ToString());
        }

        public Task<UrlsResponse> ExecuteAsync(string name)
        {
            UriQuery query = GetQuery().Add("name", name);

            return ExecuteAsync<UrlsResponse>(query.ToString());
        }

        private UriQuery GetQuery()
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey);

            return query;
        }

        #endregion Methods
    }
}