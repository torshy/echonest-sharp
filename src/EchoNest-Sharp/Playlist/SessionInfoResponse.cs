using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Playlist
{
    [DataContract]
    public class SessionInfoResponse : Response
    {
        [DataMember(Name = "terms")]
        public List<SessionInfoTerm> Terms { get; set; }

        [DataMember(Name = "description")]
        public List<string> Description { get; set; }

        [DataMember(Name = "seed_songs")]
        public List<string> SeedSongs { get; set; }

        [DataMember(Name = "banned_artists")]
        public List<string> BannedArtists { get; set; }

        [DataMember(Name = "rules")]
        public List<SessionInfoRule> Rules { get; set; }

        [DataMember(Name = "session_id")]
        public string SessionId { get; set; }

        [DataMember(Name = "seeds")]
        public List<string> Seeds { get; set; }

        [DataMember(Name = "skipped_songs")]
        public List<SessionSkippedSong> SkippedSongs { get; set; }
    }
}