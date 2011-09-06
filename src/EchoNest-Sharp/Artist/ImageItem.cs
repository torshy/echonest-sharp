using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class ImageItem
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "license")]
        public License License { get; set; }
    }
}