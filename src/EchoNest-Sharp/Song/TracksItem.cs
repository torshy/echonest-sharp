using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class TracksItem
    {
        [DataMember(Name = "release_image")]
        public string ReleaseImage { get; set; }
        [DataMember(Name = "foreign_release_id")]
        public string ForeignReleaseID { get; set; }
        [DataMember(Name = "preview_url")]
        public string PreviewUrl { get; set; }
        [DataMember(Name = "catalog")]
        public string Catalog { get; set; }
        [DataMember(Name = "foreign_id")]
        public string ForeignID { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
    }
}