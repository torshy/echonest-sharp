using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class ExtractResponse : Response
    {
        [DataMember(Name = "artists")]
        public List<ArtistBucketItem> Artists { get; set; }
    }
}