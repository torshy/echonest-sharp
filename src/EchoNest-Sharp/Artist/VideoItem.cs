using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class VideoItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "site")]
        public string Site { get; set; }
        [DataMember(Name = "date_found")]
        public string DateFound { get; set; }
        [DataMember(Name = "image_url")]
        public string ImageUrl { get; set; }
    }
}