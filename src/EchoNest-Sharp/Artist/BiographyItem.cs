using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract(Name = "biography")]
    public class BiographyItem
    {
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "site")]
        public string Site { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "license")]
        public License License { get; set; }
        [DataMember(Name = "truncated")]
        public bool? Truncated { get; set; }
    }
}