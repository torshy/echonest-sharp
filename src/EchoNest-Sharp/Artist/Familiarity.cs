using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Familiarity : EchoNestService
    {
        #region Fields

        private const string Url = "artist/familiarity";

        #endregion Fields

        #region Methods

        public FamiliarityResponse Execute(IdSpace id)
        {
            UriQuery query = GetQuery().Add("id", id);

            return Execute<FamiliarityResponse>(query.ToString());
        }

        public FamiliarityResponse Execute(string name)
        {
            UriQuery query = GetQuery().Add("name", name);

            return Execute<FamiliarityResponse>(query.ToString());
        }

        public Task<FamiliarityResponse> ExecuteAsync(IdSpace id)
        {
            UriQuery query = GetQuery().Add("id", id);

            return ExecuteAsync<FamiliarityResponse>(query.ToString());
        }

        public Task<FamiliarityResponse> ExecuteAsync(string name)
        {
            UriQuery query = GetQuery().Add("name", name);

            return ExecuteAsync<FamiliarityResponse>(query.ToString());
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