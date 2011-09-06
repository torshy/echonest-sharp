using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class HotttnesssItem
    {
        [DataMember(Name = "hotttnesss")]
        public double Hotttnesss { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}