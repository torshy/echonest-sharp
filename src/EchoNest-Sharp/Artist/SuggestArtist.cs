using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class SuggestArtist : EchoNestService
    {
        #region Fields

        private const string Url = "artist/suggest";

        #endregion Fields

        #region Methods

        public SuggestArtistResponse Execute(string artistName, int numberOfResults = 10)
        {
            string url = Build(Url)
                .Add("api_key", ApiKey)
                .Add("name", artistName)
                .Add("results", numberOfResults).ToString();

            return Execute<SuggestArtistResponse>(url);
        }

        public Task<SuggestArtistResponse> ExecuteAsync(string artistName, int numberOfResults = 10)
        {
            string url = Build(Url)
                .Add("api_key", ApiKey)
                .Add("name", artistName)
                .Add("results", numberOfResults).ToString();

            return ExecuteAsync<SuggestArtistResponse>(url);
        }

        #endregion Methods
    }
}