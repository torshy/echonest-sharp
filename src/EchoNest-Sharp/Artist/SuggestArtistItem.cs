using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class SuggestArtistItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}