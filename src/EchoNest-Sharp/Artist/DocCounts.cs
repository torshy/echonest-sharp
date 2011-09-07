using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class DocCounts
    {
        [DataMember(Name = "reviews")]
        public int Reviews { get; set; }
        [DataMember(Name = "images")]
        public int Images { get; set; }
        [DataMember(Name = "video")]
        public int Video { get; set; }
        [DataMember(Name = "biographies")]
        public int Biographies { get; set; }
        [DataMember(Name = "news")]
        public int News { get; set; }
        [DataMember(Name = "blogs")]
        public int Blogs { get; set; }
        [DataMember(Name = "audio")]
        public int Audio { get; set; }
        [DataMember(Name = "songs")]
        public int Songs { get; set; }
    }
}