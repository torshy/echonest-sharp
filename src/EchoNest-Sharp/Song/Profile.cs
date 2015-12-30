using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EchoNest.Song
{
    public class Profile : EchoNestService
    {
        #region Fields

        private const string Url = "song/profile";

        #endregion Fields

        #region Methods

        public SearchResponse Execute(ProfileArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<SearchResponse>(argument.ToString());
        }

        public Task<SearchResponse> ExecuteAsync(ProfileArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<SearchResponse>(argument.ToString());
        }

        #endregion Methods
    }
}
