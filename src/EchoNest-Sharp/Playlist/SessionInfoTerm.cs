using System.Runtime.Serialization;

namespace EchoNest.Playlist
{
    [DataContract]
    public class SessionInfoTerm
    {
        [DataMember(Name = "frequency")]
        public double Frequency { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}