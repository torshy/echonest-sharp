using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    [DebuggerDisplay("Name={Name}, ID={ID}")]
    public class ArtistBucketItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "hotttnesss")]
        public double? Hotttnesss { get; set; }
        [DataMember(Name = "familiarity")]
        public double? Familiarity { get; set; }
        [DataMember(Name = "biographies")]
        public List<BiographyItem> Biographies { get; set; }
        [DataMember(Name = "blogs")]
        public List<BlogItem> Blogs { get; set; }
        [DataMember(Name = "doc_counts")]
        public int DocCounts { get; set; }
        [DataMember(Name = "images")]
        public List<ImageItem> Images { get; set; }
        [DataMember(Name = "songs")]
        public List<SongItem> Songs { get; set; }
        [DataMember(Name = "news")]
        public List<NewsItem> News { get; set; }
        [DataMember(Name = "reviews")]
        public List<ReviewsItem> Reviews { get; set; }
        [DataMember(Name = "terms")]
        public List<TermsItem> Terms { get; set; }
        [DataMember(Name = "urls")]
        public List<UrlsItem> Urls { get; set; }
        [DataMember(Name = "video")]
        public List<VideoItem> Videos { get; set; }
        [DataMember(Name = "years_active")]
        public List<YearsActive> YearsActive { get; set; }
        [DataMember(Name = "foreign_ids")]
        public List<ForeignIdItem> ForeignIds { get; set; }
    }
}