using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class ReviewsItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "summary")]
        public string Summary { get; set; }
        [DataMember(Name = "date_found")]
        public string DateFound { get; set; }
        [DataMember(Name = "date_reviewed")]
        public string DateReviewed { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "image_url")]
        public string ImageUrl { get; set; }
        [DataMember(Name = "release")]
        public string Release { get; set; }
    }
}