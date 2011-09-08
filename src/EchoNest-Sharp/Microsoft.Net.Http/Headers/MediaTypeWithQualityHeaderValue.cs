using System;

namespace System.Net.Http.Headers
{
    public sealed class MediaTypeWithQualityHeaderValue : MediaTypeHeaderValue, ICloneable
    {
        public double? Quality
        {
            get { return HeaderUtilities.GetQuality(Parameters); }
            set { HeaderUtilities.SetQuality(Parameters, value); } 
        }

        internal MediaTypeWithQualityHeaderValue()
            : base()
        {
            // Used by the parser to create a new instance of this type.
        }

        public MediaTypeWithQualityHeaderValue(string mediaType)
            : base(mediaType)
        {
        }

        public MediaTypeWithQualityHeaderValue(string mediaType, double quality)
            : base(mediaType)
        {
            Quality = quality;
        }

        private MediaTypeWithQualityHeaderValue(MediaTypeWithQualityHeaderValue source)
            : base(source)
        {
            // No additional members to initialize here. This constructor is used by Clone().
        }

        object ICloneable.Clone()
        {
            return new MediaTypeWithQualityHeaderValue(this);
        }
    }
}
