using System;

namespace System.Net.Http.Headers
{
    public sealed class TransferCodingWithQualityHeaderValue : TransferCodingHeaderValue, ICloneable
    {
        public double? Quality
        {
            get { return HeaderUtilities.GetQuality(Parameters); }
            set { HeaderUtilities.SetQuality(Parameters, value); }
        }

        internal TransferCodingWithQualityHeaderValue()
        {
            // Used by the parser to create a new instance of this type.
        }

        public TransferCodingWithQualityHeaderValue(string value)
            : base(value)
        {
        }

        public TransferCodingWithQualityHeaderValue(string value, double quality)
            : base(value)
        {
            Quality = quality;
        }
        
        private TransferCodingWithQualityHeaderValue(TransferCodingWithQualityHeaderValue source)
            : base(source)
        { 
            // No additional members to initialize here. This constructor is used by Clone().
        }

        object ICloneable.Clone()
        {
            return new TransferCodingWithQualityHeaderValue(this);
        }
    }
}
