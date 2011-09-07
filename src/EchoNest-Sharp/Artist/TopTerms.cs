using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class TopTerms : EchoNestService
    {
        #region Fields

        private const string Url = "artist/top_terms";

        #endregion Fields

        #region Methods

        public TopTermsResponse Execute(int numberOfResults = 15)
        {
            UriQuery query = GetQuery(numberOfResults);

            return Execute<TopTermsResponse>(query.ToString());
        }

        public Task<TopTermsResponse> ExecuteAsync(int numberOfResults = 15)
        {
            UriQuery query = GetQuery(numberOfResults);

            return ExecuteAsync<TopTermsResponse>(query.ToString());
        }

        private UriQuery GetQuery(int numberOfResults)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("results", numberOfResults);

            return query;
        }

        #endregion Methods
    }
}