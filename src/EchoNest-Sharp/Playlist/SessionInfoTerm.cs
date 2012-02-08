using System.Runtime.Serialization;

namespace EchoNest.Playlist
{
    [DataContract]
    public class SessionInfoTerm
    {
        [DataMember(Name = "frequency")]
        public double Frequency { get; set; }

        [DataMember(Name = "name")]
        public double Name { get; set; }
    }
}