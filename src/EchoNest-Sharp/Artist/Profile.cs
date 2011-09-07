using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Profile : EchoNestService
    {
        #region Fields

        private const string Url = "artist/profile";

        #endregion Fields

        #region Methods

        public ProfileResponse Execute(IdSpace id, Bucket? bucket = null)
        {
            UriQuery query = GetQuery(bucket).Add("id", id);

            return Execute<ProfileResponse>(query.ToString());
        }

        public ProfileResponse Execute(string name, Bucket? bucket = null)
        {
            UriQuery query = GetQuery(bucket).Add("name", name);

            return Execute<ProfileResponse>(query.ToString());
        }

        public Task<ProfileResponse> ExecuteAsync(IdSpace id, Bucket? bucket = null)
        {
            UriQuery query = GetQuery(bucket).Add("id", id);

            return ExecuteAsync<ProfileResponse>(query.ToString());
        }

        public Task<ProfileResponse> ExecuteAsync(string name, Bucket? bucket = null)
        {
            UriQuery query = GetQuery(bucket).Add("name", name);

            return ExecuteAsync<ProfileResponse>(query.ToString());
        }

        private UriQuery GetQuery(Bucket? bucket = null)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey);

            if (bucket.HasValue)
            {
                foreach (var bucketName in bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucketName);
                }
            }

            return query;
        }

        #endregion Methods
    }
}