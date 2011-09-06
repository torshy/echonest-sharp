using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Blog : EchoNestService
    {
        #region Fields

        private const string Url = "artist/blogs";

        #endregion Fields

        #region Methods

        public BlogResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("id", id);

            return Execute<BlogResponse>(query.ToString());
        }

        public Task<BlogResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("id", id);

            return ExecuteAsync<BlogResponse>(query.ToString());
        }

        public BlogResponse Execute(string name, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("name", name);

            return Execute<BlogResponse>(query.ToString());
        }

        public Task<BlogResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0, bool highRelevance = false)
        {
            UriQuery query = GetQuery(numberOfResults, start, highRelevance);
            query.Add("name", name);

            return ExecuteAsync<BlogResponse>(query.ToString());
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