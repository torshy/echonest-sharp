using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class SearchResponse : Response
    {
        [DataMember(Name = "songs")]
        public List<SongBucketItem> Songs { get; set; }
    }
}