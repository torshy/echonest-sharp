using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class SuggestArtist : EchoNestService
    {
        #region Fields

        private const string SuggestUrl = "artist/suggest?api_key={0}&name={1}&results={2}";

        #endregion Fields

        #region Methods

        public SuggestArtistResponse Execute(string artistName, int numberOfResults = 10)
        {
            return Execute<SuggestArtistResponse>(
                SuggestUrl,
                ApiKey,
                artistName,
                numberOfResults);
        }

        public Task<SuggestArtistResponse> ExecuteAsync(string artistName, int numberOfResults = 10)
        {
            return ExecuteAsync<SuggestArtistResponse>(
                SuggestUrl,
                ApiKey,
                artistName,
                numberOfResults);
        }

        #endregion Methods
    }
}