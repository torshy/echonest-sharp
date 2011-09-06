using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace EchoNest.Shell
{
    class Program
    {
        public const string ApiKey = "XXX";
        public const string BaseUrl = "http://developer.echonest.com/api/v4/";

        static void Main(string[] args)
        {
            string suggestions = "artist/suggest?api_key={0}&name={1}&results={2}";
            suggestions = string.Format(suggestions, ApiKey, "NOFX", "10");
            HttpClient client = new HttpClient(BaseUrl);
            HttpResponseMessage resp = client.Get(suggestions);
            JsonValue biff = JsonValue.Load(resp.Content.ContentReadStream);
            var data = biff.ReadAsType<ResponseContainer<SuggestedArtistResponse>>();
            Console.WriteLine(data);
        }
    }

    [DataContract]
    public class SuggestedArtistResponse : Response
    {
        [DataMember(Name = "artists")]
        public List<SuggestedArtist> Artists { get; set; }
    }

    [DataContract]
    public class SuggestedArtist
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
