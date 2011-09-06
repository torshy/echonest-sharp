using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Image : EchoNestService
    {
        #region Fields

        private const string Url = "artist/images";

        #endregion Fields

        #region Methods

        public ImageResponse Execute(IdSpace id, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("id", id);

            return Execute<ImageResponse>(query.ToString());
        }

        public ImageResponse Execute(string name, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("name", name);

            return Execute<ImageResponse>(query.ToString());
        }

        public Task<ImageResponse> ExecuteAsync(IdSpace id, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("id", id);

            return ExecuteAsync<ImageResponse>(query.ToString());
        }

        public Task<ImageResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0, string license = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, license);
            query.Add("name", name);

            return ExecuteAsync<ImageResponse>(query.ToString());
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