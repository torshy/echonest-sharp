using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Blog : EchoNestService
    {
        #region Fields

        private const string BlogUrl = "artist/blogs?api_key={0}&name={1}&results={2}&start={3}";

        #endregion Fields

        #region Methods

        public BlogResponse Execute(string name, int numberOfResults = 10, int start = 0)
        {
            return Execute<BlogResponse>(
                BlogUrl,
                ApiKey,
                name,
                numberOfResults,
                start);
        }

        public Task<BlogResponse> ExecuteAsync(string name, int numberOfResults = 10, int start = 0)
        {
            return ExecuteAsync<BlogResponse>(
                BlogUrl,
                ApiKey,
                name,
                numberOfResults,
                start);
        }

        #endregion Methods
    }
}