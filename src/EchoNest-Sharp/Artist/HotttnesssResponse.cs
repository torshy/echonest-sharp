using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class HotttnesssResponse : Response
    {
        [DataMember(Name = "artist")]
        public HotttnesssItem Artist { get; set; }
    }
}