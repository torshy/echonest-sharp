using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Profile : EchoNestService
    {
        #region Fields

        private const string Url = "artist/profile";

        #endregion Fields

        #region Methods

        public ProfileResponse Execute(IdSpace id)
        {
            UriQuery query = GetQuery().Add("id", id);

            return Execute<ProfileResponse>(query.ToString());
        }

        public ProfileResponse Execute(string name)
        {
            UriQuery query = GetQuery().Add("name", name);

            return Execute<ProfileResponse>(query.ToString());
        }

        public Task<ProfileResponse> ExecuteAsync(IdSpace id)
        {
            UriQuery query = GetQuery().Add("id", id);

            return ExecuteAsync<ProfileResponse>(query.ToString());
        }

        public Task<ProfileResponse> ExecuteAsync(string name)
        {
            UriQuery query = GetQuery().Add("name", name);

            return ExecuteAsync<ProfileResponse>(query.ToString());
        }

        private UriQuery GetQuery()
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey);

            return query;
        }

        #endregion Methods
    }
}