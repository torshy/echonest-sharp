using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class FamiliarityItem
    {
        [DataMember(Name = "familiarity")]
        public double Familiarity { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}