using System;
using System.ComponentModel;

namespace EchoNest.Playlist
{
    [Flags]
    public enum PlaylistBucket
    {
        [Description("tracks")]
        Tracks = 1,
        [Description("id:catalog-name")]
        IdCatalogName = 2
    }
}