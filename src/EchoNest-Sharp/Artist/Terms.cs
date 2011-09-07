using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Terms : EchoNestService
    {
        #region Fields

        private const string Url = "artist/terms";

        #endregion Fields

        #region Methods

        public TermsResponse Execute(IdSpace id, TermsSort sortBy = TermsSort.Frequency)
        {
            UriQuery query = GetQuery(sortBy).Add("id", id);

            return Execute<TermsResponse>(query.ToString());
        }

        public TermsResponse Execute(string name, TermsSort sortBy = TermsSort.Frequency)
        {
            UriQuery query = GetQuery(sortBy).Add("name", name);

            return Execute<TermsResponse>(query.ToString());
        }

        public Task<TermsResponse> ExecuteAsync(IdSpace id, TermsSort sortBy = TermsSort.Frequency)
        {
            UriQuery query = GetQuery(sortBy).Add("id", id);

            return ExecuteAsync<TermsResponse>(query.ToString());
        }

        public Task<TermsResponse> ExecuteAsync(string name, TermsSort sortBy = TermsSort.Frequency)
        {
            UriQuery query = GetQuery(sortBy).Add("name", name);

            return ExecuteAsync<TermsResponse>(query.ToString());
        }

        private UriQuery GetQuery(TermsSort sortBy)
        {
            UriQuery query = Build(Url)
                .Add("api_key", ApiKey)
                .Add("sort", sortBy.ToString().ToLower());

            return query;
        }

        #endregion Methods
    }
}