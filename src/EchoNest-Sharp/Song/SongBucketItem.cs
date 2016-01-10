using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class SongBucketItem
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "artist_id")]
        public string ArtistID { get; set; }
        [DataMember(Name = "artist_name")]
        public string ArtistName { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "audio_summary")]
        public AudioSummary AudioSummary { get; set; }
        [DataMember(Name = "artist_familiarity")]
        public double ArtistFamiliarity { get; set; }
        [DataMember(Name = "artist_hotttnesss")]
        public double ArtistHotttnesss { get; set; }
        [DataMember(Name = "artist_location")]
        public ArtistLocation ArtistLocation { get; set; }
        [DataMember(Name = "song_hotttnesss")]
        public double SongHotttnesss { get; set; }
        [DataMember(Name = "tracks")]
        public List<TracksItem> Tracks { get; set; }
        [DataMember(Name = "song_type")]
        public List<string> SongType { get; set; }
        [DataMember(Name = "song_hotttnesss_rank")]
        public double? SongHotttnesssRank { get; set; }
        [DataMember(Name = "song_discovery")]
        public double? SongDiscovery { get; set; }
        [DataMember(Name = "song_discovery_rank")]
        public double? SongDiscoveryRank { get; set; }
        [DataMember(Name = "song_currency")]
        public double? SongCurrency { get; set; }
        [DataMember(Name = "song_currency_rank")]
        public double? SongCurrencyRank { get; set; }
        [DataMember(Name = "artist_foreign_ids")]
        public List<Artist.ForeignIdItem> ArtistForeignIds { get; set; }
    }
}