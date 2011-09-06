using System.Runtime.Serialization;

namespace EchoNest
{
    [DataContract]
    public class ResponseStatus
    {
        [DataMember(Name = "code")]
        public ResponseCode Code { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
    }
}