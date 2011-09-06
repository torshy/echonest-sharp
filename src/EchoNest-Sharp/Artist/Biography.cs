using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Biography : EchoNestService
    {
        #region Fields

        private const string Url = "artist/biographies";

        #endregion Fields

        #region Methods

        public BiographyResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("id", id);

            return Execute<BiographyResponse>(query.ToString());
        }

        public BiographyResponse Execute(string name, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("name", name);

            return Execute<BiographyResponse>(query.ToString());
        }

        public Task<BiographyResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("id", id);

            return ExecuteAsync<BiographyResponse>(query.ToString());
        }

        public Task<BiographyResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("name", name);

            return ExecuteAsync<BiographyResponse>(query.ToString());
        }

        private UriQuery GetQuery(int numberOfResults, int start, string license)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("results", numberOfResults)
                .Add("start", start);

            if (!string.IsNullOrEmpty(license))
            {
                query.Add("license", license);
            }

            return query;
        }

        #endregion Methods
    }
}