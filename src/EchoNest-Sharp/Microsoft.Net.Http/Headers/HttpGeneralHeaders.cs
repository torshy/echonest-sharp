using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Net.Http.Headers
{
    // The purpose of this type is to extract the handling of general headers in one place rather than duplicating
    // functionality in both HttpRequestHeaders and HttpResponseHeaders. The original approach was to have these two
    // types derive from HttpGeneralHeaders. However, in order to keep the public API surface small, it was preferred
    // to have HttpRequestHeaders & HttpResponseHeaders derive from HttpHeaders.
    internal sealed class HttpGeneralHeaders
    {
        private HttpHeaderValueCollection<string> connection;
        private HttpHeaderValueCollection<string> trailer;
        private HttpHeaderValueCollection<TransferCodingHeaderValue> transferEncoding;
        private HttpHeaderValueCollection<ProductHeaderValue> upgrade;
        private HttpHeaderValueCollection<ViaHeaderValue> via;
        private HttpHeaderValueCollection<WarningHeaderValue> warning;
        private HttpHeaderValueCollection<NameValueHeaderValue> pragma;
        private HttpHeaders parent;
        private bool transferEncodingChunkedSet;
        private bool connectionCloseSet;

        public CacheControlHeaderValue CacheControl
        {
            get { return (CacheControlHeaderValue)parent.GetParsedValues(HttpKnownHeaderNames.CacheControl); }
            set { parent.SetOrRemoveParsedValue(HttpKnownHeaderNames.CacheControl, value); }
        }

        public ICollection<string> Connection
        {
            get { return ConnectionCore; }
        }

        public bool? ConnectionClose
        {
            get
            {
                if (ConnectionCore.IsSpecialValueSet)
                {
                    return true;
                }
                if (connectionCloseSet)
                {
                    return false;
                }
                return null;
            }
            set
            {
                if (value == true)
                {
                    connectionCloseSet = true;
                    ConnectionCore.SetSpecialValue();
                }
                else
                {
                    connectionCloseSet = value != null;
                    ConnectionCore.RemoveSpecialValue();
                }
            }
        }

        public DateTimeOffset? Date
        {
            get { return HeaderUtilities.GetDateTimeOffsetValue(HttpKnownHeaderNames.Date, parent); }
            set { parent.SetOrRemoveParsedValue(HttpKnownHeaderNames.Date, value); }
        }

        public ICollection<NameValueHeaderValue> Pragma
        {
            get
            {
                if (pragma == null)
                {
                    pragma = new HttpHeaderValueCollection<NameValueHeaderValue>(HttpKnownHeaderNames.Pragma, parent);
                }
                return pragma;
            }
        }

        public ICollection<string> Trailer
        {
            get
            {
                if (trailer == null)
                {
                    trailer = new HttpHeaderValueCollection<string>(HttpKnownHeaderNames.Trailer,
                        parent, HeaderUtilities.TokenValidator);
                }
                return trailer;
            }
        }

        public ICollection<TransferCodingHeaderValue> TransferEncoding
        {
            get { return TransferEncodingCore; }
        }

        public bool? TransferEncodingChunked
        {
            get
            {
                if (TransferEncodingCore.IsSpecialValueSet)
                {
                    return true;
                }
                if (transferEncodingChunkedSet)
                {
                    return false;
                }
                return null;
            }
            set
            {
                if (value == true)
                {
                    transferEncodingChunkedSet = true;
                    TransferEncodingCore.SetSpecialValue();
                }
                else
                {
                    transferEncodingChunkedSet = value != null;
                    TransferEncodingCore.RemoveSpecialValue();
                }
            }
        }

        public ICollection<ProductHeaderValue> Upgrade
        {
            get
            {
                if (upgrade == null)
                {
                    upgrade = new HttpHeaderValueCollection<ProductHeaderValue>(HttpKnownHeaderNames.Upgrade, parent);
                }
                return upgrade;
            }
        }

        public ICollection<ViaHeaderValue> Via
        {
            get
            {
                if (via == null)
                {
                    via = new HttpHeaderValueCollection<ViaHeaderValue>(HttpKnownHeaderNames.Via, parent);
                }
                return via;
            }
        }

        public ICollection<WarningHeaderValue> Warning
        {
            get
            {
                if (warning == null)
                {
                    warning = new HttpHeaderValueCollection<WarningHeaderValue>(HttpKnownHeaderNames.Warning, parent);
                }
                return warning;
            }
        }

        private HttpHeaderValueCollection<string> ConnectionCore
        {
            get
            {
                if (connection == null)
                {
                    connection = new HttpHeaderValueCollection<string>(HttpKnownHeaderNames.Connection,
                        parent, HeaderUtilities.ConnectionClose, HeaderUtilities.CaseInsensitiveStringComparer,
                        HeaderUtilities.TokenValidator);
                }
                return connection;
            }
        }

        private HttpHeaderValueCollection<TransferCodingHeaderValue> TransferEncodingCore
        {
            get
            {
                if (transferEncoding == null)
                {
                    transferEncoding = new HttpHeaderValueCollection<TransferCodingHeaderValue>(
                        HttpKnownHeaderNames.TransferEncoding, parent, HeaderUtilities.TransferEncodingChunked,
                        null);
                }
                return transferEncoding;
            }
        }

        internal HttpGeneralHeaders(HttpHeaders parent)
        {
            Contract.Requires(parent != null);

            this.parent = parent;
        }

        internal static void AddParsers(Dictionary<string, HttpHeaderParser> parserStore)
        {
            Contract.Requires(parserStore != null);

            parserStore.Add(HttpKnownHeaderNames.CacheControl, CacheControlHeaderParser.Parser);
            parserStore.Add(HttpKnownHeaderNames.Connection, GenericHeaderParser.TokenListParser);
            parserStore.Add(HttpKnownHeaderNames.Date, DateHeaderParser.Parser);
            parserStore.Add(HttpKnownHeaderNames.Pragma, GenericHeaderParser.NameValueParser);
            parserStore.Add(HttpKnownHeaderNames.Trailer, GenericHeaderParser.TokenListParser);
            parserStore.Add(HttpKnownHeaderNames.TransferEncoding, TransferCodingHeaderParser.Parser);
            parserStore.Add(HttpKnownHeaderNames.Upgrade, GenericHeaderParser.ProductParser);
            parserStore.Add(HttpKnownHeaderNames.Via, GenericHeaderParser.ViaParser);
            parserStore.Add(HttpKnownHeaderNames.Warning, GenericHeaderParser.WarningParser);
        }

        internal static void AddKnownHeaders(HashSet<string> headerSet)
        {
            Contract.Requires(headerSet != null);

            headerSet.Add(HttpKnownHeaderNames.CacheControl);
            headerSet.Add(HttpKnownHeaderNames.Connection);
            headerSet.Add(HttpKnownHeaderNames.Date);
            headerSet.Add(HttpKnownHeaderNames.Pragma);
            headerSet.Add(HttpKnownHeaderNames.Trailer);
            headerSet.Add(HttpKnownHeaderNames.TransferEncoding);
            headerSet.Add(HttpKnownHeaderNames.Upgrade);
            headerSet.Add(HttpKnownHeaderNames.Via);
            headerSet.Add(HttpKnownHeaderNames.Warning);
        }
    }
}
