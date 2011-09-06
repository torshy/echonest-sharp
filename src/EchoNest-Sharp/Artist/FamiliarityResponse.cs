using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class FamiliarityResponse : Response
    {
        [DataMember(Name = "artist")]
        public FamiliarityItem Artist { get; set; }
    }
}