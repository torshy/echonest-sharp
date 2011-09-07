using System.Diagnostics;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    [DebuggerDisplay("Name={Name}, Frequency={Frequency}, Weight={Weight}")]
    public class TermsItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "frequency")]
        public double Frequency { get; set; }
        [DataMember(Name = "weight")]
        public double Weight { get; set; }
    }
}