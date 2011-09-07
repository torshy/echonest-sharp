using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class SearchResponse : Response
    {
        [DataMember(Name = "artists")]
        public List<ArtistBucketItem> Artists { get; set; }
    }
}