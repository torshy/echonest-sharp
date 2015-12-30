using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EchoNest.Song
{
    [DataContract]
    public class ProfileResponse : Response
    {
        [DataMember(Name = "songs")]
        public List<SongBucketItem> Songs { get; set; }
    }
}
