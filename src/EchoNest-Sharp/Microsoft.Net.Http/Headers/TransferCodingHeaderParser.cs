using System.Diagnostics.Contracts;

namespace System.Net.Http.Headers
{
    internal class TransferCodingHeaderParser : BaseHeaderParser
    {
        private Func<TransferCodingHeaderValue> transferCodingCreator;

        internal static readonly TransferCodingHeaderParser Parser = new TransferCodingHeaderParser(
            CreateTransferCoding);
        internal static readonly TransferCodingHeaderParser ValueWithQualityParser = new TransferCodingHeaderParser(
            CreateTransferCodingWithQuality);

        private TransferCodingHeaderParser(Func<TransferCodingHeaderValue> transferCodingCreator)
            : base(true)
        {
            Contract.Requires(transferCodingCreator != null);

            this.transferCodingCreator = transferCodingCreator;
        }

        protected override int GetParsedValueLength(string value, int startIndex, object storeValue, 
            out object parsedValue)
        {
            TransferCodingHeaderValue temp = null;
            int resultLength = TransferCodingHeaderValue.GetTransferCodingLength(value, startIndex, 
                transferCodingCreator, out temp);

            parsedValue = temp;
            return resultLength;
        }
        
        private static TransferCodingHeaderValue CreateTransferCoding()
        {
            return new TransferCodingHeaderValue();
        }

        private static TransferCodingHeaderValue CreateTransferCodingWithQuality()
        {
            return new TransferCodingWithQualityHeaderValue();
        }
    }
}
