using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class License
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "attribution")]
        public string Attribution { get; set; }
        [DataMember(Name = "attribution-url")]
        public string AttributionUrl { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
    }
}