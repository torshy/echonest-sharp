using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class News : EchoNestService
    {
        #region Fields

        private const string Url = "artist/news";

        #endregion Fields

        #region Methods

        public NewsResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("id", id);

            return Execute<NewsResponse>(query.ToString());
        }

        public Task<NewsResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("id", id);

            return ExecuteAsync<NewsResponse>(query.ToString());
        }

        public NewsResponse Execute(string name, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("name", name);

            return Execute<NewsResponse>(query.ToString());
        }

        public Task<NewsResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("name", name);

            return ExecuteAsync<NewsResponse>(query.ToString());
        }

        private UriQuery GetQuery(int numberOfResults, int start, bool highRelevance)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("results", numberOfResults)
                .Add("start", start)
                .Add("high_relevance", highRelevance.ToString().ToLower());

            return query;
        }

        #endregion Methods
    }
}