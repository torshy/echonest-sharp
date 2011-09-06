using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class ListTerms : EchoNestService
    {
        #region Fields

        private const string Url = "artist/list_terms";

        #endregion Fields

        #region Methods

        public ListTermsResponse Execute(ListTermsType type = ListTermsType.Style)
        {
            UriQuery query = GetQuery(type);

            return Execute<ListTermsResponse>(query.ToString());
        }

        public Task<ListTermsResponse> ExecuteAsync(ListTermsType type = ListTermsType.Style)
        {
            UriQuery query = GetQuery(type);

            return ExecuteAsync<ListTermsResponse>(query.ToString());
        }
        
        private UriQuery GetQuery(ListTermsType type)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("type", type.ToString().ToLower());

            return query;
        }

        #endregion Methods
    }
}