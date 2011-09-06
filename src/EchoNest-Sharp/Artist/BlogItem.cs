using System;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class BlogItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "summary")]
        public string Summary { get; set; }
        [DataMember(Name = "datefound")]
        public DateTime DateFound { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "dateposted")]
        public DateTime DatePosted { get; set; }
    }
}