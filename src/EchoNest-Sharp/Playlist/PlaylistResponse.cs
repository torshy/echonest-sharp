using System.Collections.Generic;
using System.Runtime.Serialization;
using EchoNest.Song;

namespace EchoNest.Playlist
{
    [DataContract]
    public class PlaylistResponse : Response
    {
        [DataMember(Name = "songs")]
        public List<SongBucketItem> Songs { get; set; }
    }
}