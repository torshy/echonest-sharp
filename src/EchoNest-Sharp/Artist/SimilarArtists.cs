using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class SimilarArtists : EchoNestService
    {
        #region Fields

        private const string Url = "artist/similar";

        #endregion Fields

        #region Methods

        public SimilarArtistsResponse Execute(SimilarArtistsArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<SimilarArtistsResponse>(argument.ToString());
        }

        public Task<SimilarArtistsResponse> ExecuteAsync(SimilarArtistsArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<SimilarArtistsResponse>(argument.ToString());
        }

        #endregion Methods
    }
}