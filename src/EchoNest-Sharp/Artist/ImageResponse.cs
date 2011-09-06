using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class ImageResponse : Response
    {
        [DataMember(Name = "start")]
        public int Start { get; set; }
        [DataMember(Name = "total")]
        public int Total { get; set; }
        [DataMember(Name = "images")]
        public List<ImageItem> Images { get; set; }
    }
}