using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class ArtistLocation
    {
        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }
        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }
        [DataMember(Name = "location")]
        public string Location { get; set; }
    }
}