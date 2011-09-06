using System.Runtime.Serialization;

namespace EchoNest
{
    [DataContract]
    public class ResponseContainer<T>
    {
        [DataMember(Name = "response")]
        public T Response { get; set; }
    }
}