using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EchoNest.Song;

namespace EchoNest.Track
{
    [DataContract]
    public class TrackItem
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "song_id")]
        public string SongId { get; set; }

        [DataMember(Name = "artist")]
        public string Artist { get; set; }

        [DataMember(Name = "foreign_ids")]
        public List<string> ForeignIDs { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "foreign_release_ids")]
        public List<string> ForeignReleaseIDs { get; set; }

        [DataMember(Name = "foreign_release_id")]
        public string ForeignReleaseID { get; set; }
        
        [DataMember(Name = "catalog")]
        public string Catalog { get; set; }

        [DataMember(Name = "release")]
        public string Release { get; set; }

        [DataMember(Name = "audio_md5")]
        public string AudioMd5 { get; set; }

        [DataMember(Name = "foreign_id")]
        public string ForeignID { get; set; }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "release_image")]
        public string ReleaseImage { get; set; }

        [DataMember(Name = "preview_url")]
        public string PreviewUrl { get; set; }

        [DataMember(Name = "audio_summary")]
        public AudioSummary AudioSummary { get; set; }
    }
}
