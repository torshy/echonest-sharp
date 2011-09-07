using System.Diagnostics;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    [DebuggerDisplay("Title={Title}, ID={ID}")]
    public class SongItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}