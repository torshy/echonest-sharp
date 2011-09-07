using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Extract : EchoNestService
    {
        #region Fields

        private const string Url = "artist/extract";

        #endregion Fields

        #region Methods

        public ExtractResponse Execute(ExtractArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<ExtractResponse>(argument.ToString());
        }

        public Task<ExtractResponse> ExecuteAsync(ExtractArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<ExtractResponse>(argument.ToString());
        }

        #endregion Methods
    }
}