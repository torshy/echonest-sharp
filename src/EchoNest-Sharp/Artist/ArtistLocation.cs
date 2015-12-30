using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EchoNest.Artist
{
    [DataContract]
    public class ArtistLocation
    {
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "country")]
        public string Country { get; set; }
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "region")]
        public string Region { get; set; }
    }
}
