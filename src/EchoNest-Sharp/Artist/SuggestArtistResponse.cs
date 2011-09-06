using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class SuggestArtistResponse : Response
    {
        [DataMember(Name = "artists")]
        public List<SuggestArtistItem> Artists { get; set; }
    }
}