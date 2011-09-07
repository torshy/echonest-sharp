using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Reviews : EchoNestService
    {
        #region Fields

        private const string Url = "artist/reviews";

        #endregion Fields

        #region Methods

        public ReviewsResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("id", id);

            return Execute<ReviewsResponse>(query.ToString());
        }

        public Task<ReviewsResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("id", id);

            return ExecuteAsync<ReviewsResponse>(query.ToString());
        }

        public ReviewsResponse Execute(string name, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("name", name);

            return Execute<ReviewsResponse>(query.ToString());
        }

        public Task<ReviewsResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("name", name);

            return ExecuteAsync<ReviewsResponse>(query.ToString());
        }

        private UriQuery GetQuery(int numberOfResults, int start)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("results", numberOfResults)
                .Add("start", start);

            return query;
        }

        #endregion Methods
    }
}