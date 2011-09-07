using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class ProfileResponse : Response
    {
        [DataMember(Name = "artist")]
        public ProfileItem Artist { get; set; }
    }
}