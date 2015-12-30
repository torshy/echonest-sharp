using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoNest.Track
{
    public class Profile : EchoNestService
    {
        #region Fields

        private const string Url = "track/profile";

        #endregion Fields

        #region Methods

        public ProfileResponse Execute(ProfileArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<ProfileResponse>(argument.ToString());
        }

        public Task<ProfileResponse> ExecuteAsync(ProfileArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<ProfileResponse>(argument.ToString());
        }

        #endregion Methods
    }
}
