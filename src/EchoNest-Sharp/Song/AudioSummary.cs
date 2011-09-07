using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class AudioSummary
    {
        [DataMember(Name = "key")]
        public int Key { get; set; }
        [DataMember(Name = "mode")]
        public int Mode { get; set; }
        [DataMember(Name = "time_signature")]
        public int TimeSignature { get; set; }
        [DataMember(Name = "duration")]
        public double Duration { get; set; }
        [DataMember(Name = "loudness")]
        public double Loudness { get; set; }
        [DataMember(Name = "energy")]
        public double Energy { get; set; }
        [DataMember(Name = "tempo")]
        public double Tempo { get; set; }
        [DataMember(Name = "audio_md5")]
        public string AudioMd5 { get; set; }
        [DataMember(Name = "analysis_url")]
        public string AnalysisUrl { get; set; }
        [DataMember(Name = "danceability")]
        public double Danceability { get; set; }
    }
}