using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class TopTermsItem
    {
        [DataMember(Name = "frequency")]
        public double Frequency { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}