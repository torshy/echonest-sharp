using System.Runtime.Serialization;

namespace EchoNest.Playlist
{
    [DataContract]
    public class SessionInfoRule
    {
        [DataMember(Name = "rule")]
        public string Rule { get; set; }
    }
}