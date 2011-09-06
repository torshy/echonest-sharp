using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Hotttnesss : EchoNestService
    {
        #region Fields

        private const string Url = "artist/hotttnesss";

        #endregion Fields

        #region Methods

        public HotttnesssResponse Execute(IdSpace id, HotttnesssType type = HotttnesssType.Overall)
        {
            UriQuery query = GetQuery(type).Add("id", id);

            return Execute<HotttnesssResponse>(query.ToString());
        }

        public HotttnesssResponse Execute(string name, HotttnesssType type = HotttnesssType.Overall)
        {
            UriQuery query = GetQuery(type).Add("name", name);

            return Execute<HotttnesssResponse>(query.ToString());
        }

        public Task<HotttnesssResponse> ExecuteAsync(IdSpace id, HotttnesssType type = HotttnesssType.Overall)
        {
            UriQuery query = GetQuery(type).Add("id", id);

            return ExecuteAsync<HotttnesssResponse>(query.ToString());
        }

        public Task<HotttnesssResponse> ExecuteAsync(string name, HotttnesssType type = HotttnesssType.Overall)
        {
            UriQuery query = GetQuery(type).Add("name", name);

            return ExecuteAsync<HotttnesssResponse>(query.ToString());
        }

        private UriQuery GetQuery(HotttnesssType type)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("type", type.ToString().ToLower());

            return query;
        }

        #endregion Methods
    }
}