using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class SimilarArtistsResponse : Response
    {
        [DataMember(Name = "artists")]
        public List<ArtistBucket> Artists{ get; set; }
    }
}