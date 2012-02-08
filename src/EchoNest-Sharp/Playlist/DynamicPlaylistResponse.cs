using System.Runtime.Serialization;

namespace EchoNest.Playlist
{
    [DataContract]
    public class DynamicPlaylistResponse : PlaylistResponse
    {
        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }
    }
}