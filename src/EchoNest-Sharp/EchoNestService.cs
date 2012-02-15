using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EchoNest
{
    public abstract class EchoNestService
    {
        #region Properties

        public string ApiKey
        {
            get;
            internal set;
        }

        internal HttpClient HttpClient
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        protected UriQuery Build(string url)
        {
            return new UriQuery(url);
        }

        protected T Execute<T>(string url, params object[] arguments)
            where T : class
        {
            url = string.Format(url, arguments);
            HttpResponseMessage response = HttpClient.GetAsync(url).Result;
            return GetObject<T>(response);
        }

        protected Task<T> ExecuteAsync<T>(string url, params object[] arguments)
            where T : class
        {
            url = string.Format(url, arguments);
            return HttpClient.GetAsync(url).ContinueWith(new Func<Task<HttpResponseMessage>, T>(OnExecute<T>));
        }

        private T GetObject<T>(HttpResponseMessage resp)
            where T : class
        {
            var jsonString = resp.Content.ReadAsStringAsync().Result;
            var responseContainer = JsonConvert.DeserializeObject<ResponseContainer<T>>(jsonString);

            if (responseContainer != null)
            {
                return responseContainer.Response;
            }

            return null;
        }

        private T OnExecute<T>(Task<HttpResponseMessage> arg)
            where T : class
        {
            return GetObject<T>(arg.Result);
        }

        #endregion Methods
    }
}