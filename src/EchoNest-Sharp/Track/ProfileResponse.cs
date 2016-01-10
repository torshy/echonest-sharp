using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EchoNest.Track
{
    [DataContract]
    public class ProfileResponse : Response
    {
        [DataMember(Name = "track")]
        public TrackItem Track { get; set; }
    }
}
