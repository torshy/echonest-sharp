using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EchoNest.Track
{
    [Flags]
    public enum TrackBucket
    {
        [Description("audio_summary")]
        AudioSummary = 1
    }
}
