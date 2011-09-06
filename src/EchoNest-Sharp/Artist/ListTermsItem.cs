using System.Diagnostics;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    [DebuggerDisplay("{Name}")]
    public class ListTermsItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}