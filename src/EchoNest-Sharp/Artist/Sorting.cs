using System.ComponentModel;

namespace EchoNest.Artist
{
    public enum Sorting
    {
        [Description("familiarity-asc")]
        FamiliarityAsc,
        [Description("familiarity-desc")]
        FamiliarityDesc,
        [Description("hotttnesss-asc")]
        HotttnesssAsc,
        [Description("hotttnesss-desc")]
        HotttnesssDesc,
        [Description("artist_start_year-asc")]
        ArtistStartYearAsc,
        [Description("artist_start_year-desc")]
        ArtistStartYearDesc,
        [Description("artist_end_year-asc")]
        ArtistEndYearAsc,
        [Description("artist_end_year-desc")]
        ArtistEndYearDesc
    }
}