using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class UrlsResponse : Response
    {
        [DataMember(Name = "urls")]
        public UrlsItem Urls { get; set; }
    }
}