using System.Diagnostics;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    [DebuggerDisplay("Catalog={Catalog}, ID={ID}")]
    public class ForeignIdItem
    {
        [DataMember(Name = "catalog")]
        public string Catalog { get; set; }
        [DataMember(Name = "foreign_id")]
        public string ID { get; set; }
    }
}