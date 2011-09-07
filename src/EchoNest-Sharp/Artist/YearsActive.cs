using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class YearsActive
    {
        [DataMember(Name = "start")]
        public int Start { get; set; }
        [DataMember(Name = "End")]
        public int? End { get; set; }
    }
}