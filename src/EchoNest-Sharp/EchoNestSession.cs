using System;
using System.Net.Http;
using EchoNest.Artist;

namespace EchoNest
{
    public sealed class EchoNestSession : IDisposable
    {
        private readonly string _apiKey;
        private readonly string _baseUrl = "http://developer.echonest.com/api/v4/";
        private HttpClient _httpClient;

        public EchoNestSession(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient(_baseUrl);
        }

        void  IDisposable.Dispose()
        {            
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }

        public T Query<T>() where T : EchoNestService, new()
        {
            return new T
                       {
                           ApiKey = _apiKey,
                           HttpClient = _httpClient
                       };
        }
    }

}