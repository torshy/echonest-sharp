using System;
using System.Net.Http;

namespace EchoNest
{
    public sealed class EchoNestSession : IDisposable
    {
        #region Fields

        private readonly string _apiKey;
        private readonly string _baseUrl = "http://developer.echonest.com/api/v4/";

        private HttpClient _httpClient;

        #endregion Fields

        #region Constructors

        public EchoNestSession(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient(_baseUrl);
        }

        #endregion Constructors

        #region Methods

        void IDisposable.Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }

        public T Query<T>()
            where T : EchoNestService, new()
        {
            return new T
                       {
                           ApiKey = _apiKey,
                           HttpClient = _httpClient
                       };
        }

        #endregion Methods
    }
}