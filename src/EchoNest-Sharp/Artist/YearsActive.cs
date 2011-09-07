using System.Diagnostics;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    [DebuggerDisplay("Start={Start}, End={End}")]
    public class YearsActive
    {
        [DataMember(Name = "start")]
        public int Start { get; set; }
        [DataMember(Name = "end")]
        public int? End { get; set; }
    }
}