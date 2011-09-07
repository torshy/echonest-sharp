using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class TopHottt : EchoNestService
    {
        #region Fields

        private const string Url = "artist/top_hottt";

        #endregion Fields

        #region Methods

        public TopHotttResponse Execute(int numberOfResults = 15, int start = 0, ArtistBucket? bucket = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, bucket);

            return Execute<TopHotttResponse>(query.ToString());
        }

        public Task<TopHotttResponse> ExecuteAsync(int numberOfResults = 15, int start = 0, ArtistBucket? bucket = null)
        {
            UriQuery query = GetQuery(numberOfResults, start, bucket);

            return ExecuteAsync<TopHotttResponse>(query.ToString());
        }

        private UriQuery GetQuery(int numberOfResults, int start, ArtistBucket? bucket)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("results", numberOfResults)
                .Add("start", start);

            if (bucket.HasValue)
            {
                foreach (var bucketString in bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucketString);
                }
            }

            return query;
        }

        #endregion Methods
    }
}