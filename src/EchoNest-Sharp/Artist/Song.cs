using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Song : EchoNestService
    {
        #region Fields

        private const string Url = "artist/songs";

        #endregion Fields

        #region Methods

        public SongResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("id", id);

            return Execute<SongResponse>(query.ToString());
        }

        public SongResponse Execute(string name, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("name", name);

            return Execute<SongResponse>(query.ToString());
        }

        public Task<SongResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("id", id);

            return ExecuteAsync<SongResponse>(query.ToString());
        }

        public Task<SongResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0)
        {
            UriQuery query = GetQuery(numberOfResults, start);
            query.Add("name", name);

            return ExecuteAsync<SongResponse>(query.ToString());
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