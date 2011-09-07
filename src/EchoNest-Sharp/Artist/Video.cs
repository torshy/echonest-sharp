using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Video : EchoNestService
    {
        #region Fields

        private const string Url = "artist/video";

        #endregion Fields

        #region Methods

        public VideoResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("id", id);

            return Execute<VideoResponse>(query.ToString());
        }

        public VideoResponse Execute(string name, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("name", name);

            return Execute<VideoResponse>(query.ToString());
        }

        public Task<VideoResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("id", id);

            return ExecuteAsync<VideoResponse>(query.ToString());
        }

        public Task<VideoResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("name", name);

            return ExecuteAsync<VideoResponse>(query.ToString());
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