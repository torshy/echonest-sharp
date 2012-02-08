using System.Runtime.Serialization;

namespace EchoNest.Playlist
{
    [DataContract]
    public class SessionSkippedSong
    {
        [DataMember(Name = "served_time")]
        public double ServedTime { get; set; }

        [DataMember(Name = "artist_id")]
        public string ArtistId { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "artist_name")]
        public string ArtistName { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}