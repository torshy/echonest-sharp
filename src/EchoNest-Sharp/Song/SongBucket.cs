using System;
using System.ComponentModel;

namespace EchoNest.Song
{
    [Flags]
    public enum SongBucket
    {
        [Description("audio_summary")]
        AudioSummary = 1,
        [Description("artist_familiarity")]
        ArtistFamiliarity = 2,
        [Description("artist_hotttnesss")]
        ArtistHotttness = 4,
        [Description("artist_location")]
        ArtistLocation = 8,
        [Description("song_hotttnesss")]
        SongHotttness = 16,
        [Description("tracks")]
        Tracks = 32,
        [Description("artist_discovery")]
        ArtistDiscovery = 64,
        [Description("artist_discovery_rank")]
        ArtistDiscoveryRank = 128,
        [Description("artist_familiarity_rank")]
        ArtistFamiliarityRank = 256,
        [Description("artist_hotttnesss_rank")]
        ArtistHotttnesssRank = 512,
        [Description("song_currency")]
        SongCurrency = 1024,
        [Description("song_currency_rank")]
        SongCurrencyRank = 2048,
        [Description("song_hotttnesss_rank")]
        SongHotttnesssRank = 4096,
        [Description("song_type")]
        SongType = 8192,
        [Description("id:musicbrainz")]
        IdMusicBrainz = 16384,
        [Description("id:playme")]
        IdPlayme = 32768,
        [Description("id:7digital")]
        Id7digital = 65536,
        [Description("id:rdio-us-streaming")]
        IdRdioUsStreaming = 131072,
        [Description("id:spotify-WW")]
        IdSpotifyWw = 262144,
        [Description("id:deezer")]
        IdDeezer = 524288,
        [Description("song_discovery")]
        SongDiscovery = 1048576,
        [Description("song_discovery_rank")]
        SongDiscoveryRank = 2097152
    }
}
