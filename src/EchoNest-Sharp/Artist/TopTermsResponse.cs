using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class TopTermsResponse : Response
    {
        [DataMember(Name = "terms")]
        public List<TopTermsItem> Terms { get; set; }
    }
}