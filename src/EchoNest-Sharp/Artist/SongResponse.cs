using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class SongResponse : Response
    {
        [DataMember(Name = "start")]
        public int Start { get; set; }
        [DataMember(Name = "total")]
        public int Total { get; set; }
        [DataMember(Name = "songs")]
        public List<SongItem> Songs { get; set; }
    }
}