using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Biography : EchoNestService
    {
        #region Fields

        private const string BiographyUrl = 
            "artist/biographies?api_key={0}&name={1}&results={2}&start={3}";

        #endregion Fields

        #region Methods

        public BiographyResponse Execute(string name, int numberOfResults = 10, int start = 0)
        {
            return Execute<BiographyResponse>(
                BiographyUrl,
                ApiKey,
                name,
                numberOfResults,
                start);
        }

        public Task<BiographyResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0)
        {
            return ExecuteAsync<BiographyResponse>(
                BiographyUrl,
                ApiKey,
                name,
                numberOfResults,
                start);
        }

        #endregion Methods
    }
}