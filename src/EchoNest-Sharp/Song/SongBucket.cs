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
        [Description("id:musicbrainz")]
        IdMusicBrainz = 16384,
        [Description("id:playme")]
        IdPlayme = 32768,
        [Description("id:7digital")]
        Id7digital = 65536,
        [Description("id:rdio-us-streaming")]
        IdRdioUsStreaming = 131072
    }
}
