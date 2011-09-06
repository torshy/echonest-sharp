using System.Runtime.Serialization;

namespace EchoNest
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "status")]
        public ResponseStatus Status { get; set; }
    }
}