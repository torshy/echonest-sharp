using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Suggest : EchoNestService
    {
        #region Fields

        private const string SuggestUrl = "artist/suggest?api_key={0}&name={1}&results={2}";

        #endregion Fields

        #region Methods

        public SuggestResponse Execute(string artistName, int numberOfResults = 10)
        {
            return Execute<SuggestResponse>(
                SuggestUrl,
                ApiKey,
                artistName,
                numberOfResults);
        }

        public Task<SuggestResponse> ExecuteAsync(string artistName, int numberOfResults = 10)
        {
            return ExecuteAsync<SuggestResponse>(
                SuggestUrl,
                ApiKey,
                artistName,
                numberOfResults);
        }

        #endregion Methods
    }
}