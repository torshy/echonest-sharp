using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class TermsResponse : Response
    {
        [DataMember(Name = "terms")]
        public List<TermsItem> Terms { get; set; }
    }
}