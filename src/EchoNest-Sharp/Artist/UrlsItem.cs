using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class UrlsItem
    {
        [DataMember(Name = "lastfm_url")]
        public string LastFm { get; set; }
        [DataMember(Name = "aolmusic_url")]
        public string AolMusic { get; set; }
        [DataMember(Name = "myspace_url")]
        public string MySpace { get; set; }
        [DataMember(Name = "amazon_url")]
        public string Amazon { get; set; }
        [DataMember(Name = "itunes_url")]
        public string Itunes { get; set; }
        [DataMember(Name = "mb_url")]
        public string MusicBrainz { get; set; }
    }
}