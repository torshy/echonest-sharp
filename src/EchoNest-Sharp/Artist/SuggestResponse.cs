using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class SuggestResponse : Response
    {
        [DataMember(Name = "artists")]
        public List<SuggestArtist> Artists { get; set; }
    }
}