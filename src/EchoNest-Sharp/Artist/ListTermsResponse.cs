using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class ListTermsResponse : Response
    {
        [DataMember(Name = "type")]
        public string TypeAsString { get; set; }
        [DataMember(Name = "terms")]
        public List<ListTermsItem> Terms { get; set; }

        public ListTermsType Type
        {
            get
            {
                ListTermsType type;
                Enum.TryParse(TypeAsString, true, out type);
                return type;
            }
        }
    }
}