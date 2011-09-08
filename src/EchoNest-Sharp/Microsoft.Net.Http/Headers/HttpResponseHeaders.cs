using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Net.Http.Headers
{
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "This is not a collection")]
    public sealed class HttpResponseHeaders : HttpHeaders
    {
        private static readonly Dictionary<string, HttpHeaderParser> parserStore;
        private static readonly HashSet<string> invalidHeaders;

        private HttpGeneralHeaders generalHeaders;
        private HttpHeaderValueCollection<string> acceptRanges;
        private HttpHeaderValueCollection<AuthenticationHeaderValue> wwwAuthenticate;
        private HttpHeaderValueCollection<AuthenticationHeaderValue> proxyAuthenticate;
        private HttpHeaderValueCollection<ProductInfoHeaderValue> server;
        private HttpHeaderValueCollection<string> vary;

        #region Response Headers

        public ICollection<string> AcceptRanges
        {
            get
            {
                if (acceptRanges == null)
                {
                    acceptRanges = new HttpHeaderValueCollection<string>(HttpKnownHeaderNames.AcceptRanges,
                        this, HeaderUtilities.TokenValidator);
                }
                return acceptRanges;
            }
        }

        public TimeSpan? Age
        {
            get { return HeaderUtilities.GetTimeSpanValue(HttpKnownHeaderNames.Age, this); }
            set { SetOrRemoveParsedValue(HttpKnownHeaderNames.Age, value); }
        }

        public EntityTagHeaderValue ETag
        {
            get { return (EntityTagHeaderValue)GetParsedValues(HttpKnownHeaderNames.ETag); }
            set { SetOrRemoveParsedValue(HttpKnownHeaderNames.ETag, value); }
        }

        public Uri Location
        {
            get { return (Uri)GetParsedValues(HttpKnownHeaderNames.Location); }
            // The RFC says that the Location header should be an absolute Uri, 
            // but IIS and HttpListener do not enforce this.
            set { SetOrRemoveParsedValue(HttpKnownHeaderNames.Location, value); }
        }

        public ICollection<AuthenticationHeaderValue> ProxyAuthenticate
        {
            get
            {
                if (proxyAuthenticate == null)
                {
                    proxyAuthenticate = new HttpHeaderValueCollection<AuthenticationHeaderValue>(
                        HttpKnownHeaderNames.ProxyAuthenticate, this);
                }
                return proxyAuthenticate;
            }
        }

        public RetryConditionHeaderValue RetryAfter
        {
            get { return (RetryConditionHeaderValue)GetParsedValues(HttpKnownHeaderNames.RetryAfter); }
            set { SetOrRemoveParsedValue(HttpKnownHeaderNames.RetryAfter, value); }
        }

        public ICollection<ProductInfoHeaderValue> Server
        {
            get
            {
                if (server == null)
                {
                    server = new HttpHeaderValueCollection<ProductInfoHeaderValue>(HttpKnownHeaderNames.Server, this);
                }
                return server;
            }
        }

        public ICollection<string> Vary
                {
            get
            {
                if (vary == null)
                {
                    vary = new HttpHeaderValueCollection<string>(HttpKnownHeaderNames.Vary,
                        this, HeaderUtilities.TokenValidator);
                }
                return vary;
            }
        }

        public ICollection<AuthenticationHeaderValue> WwwAuthenticate
        {
            get
            {
                if (wwwAuthenticate == null)
                {
                    wwwAuthenticate = new HttpHeaderValueCollection<AuthenticationHeaderValue>(
                        HttpKnownHeaderNames.WWWAuthenticate, this);
                }
                return wwwAuthenticate;
            }
        }

        #endregion

        #region General Headers

        public CacheControlHeaderValue CacheControl
        {
            get { return generalHeaders.CacheControl; }
            set { generalHeaders.CacheControl = value; }
        }

        public ICollection<string> Connection
        {
            get { return generalHeaders.Connection; }
        }

        public bool? ConnectionClose
        {
            get { return generalHeaders.ConnectionClose; }
            set { generalHeaders.ConnectionClose = value; }
        }

        public DateTimeOffset? Date
        {
            get { return generalHeaders.Date; }
            set { generalHeaders.Date = value; }
        }

        public ICollection<NameValueHeaderValue> Pragma
        {
            get { return generalHeaders.Pragma; }
        }

        public ICollection<string> Trailer
        {
            get { return generalHeaders.Trailer; }
        }

        // Like ContentEncoding: Order matters!
        public ICollection<TransferCodingHeaderValue> TransferEncoding
        {
            get { return generalHeaders.TransferEncoding; }
        }

        public bool? TransferEncodingChunked
        {
            get { return generalHeaders.TransferEncodingChunked; }
            set { generalHeaders.TransferEncodingChunked = value; }
        }

        public ICollection<ProductHeaderValue> Upgrade
        {
            get { return generalHeaders.Upgrade; }
        }

        public ICollection<ViaHeaderValue> Via
        {
            get { return generalHeaders.Via; }
        }

        public ICollection<WarningHeaderValue> Warning
        {
            get { return generalHeaders.Warning; }
        }

        #endregion

        internal HttpResponseHeaders()
        {
            this.generalHeaders = new HttpGeneralHeaders(this);

            base.SetConfiguration(parserStore, invalidHeaders);
        }

        static HttpResponseHeaders()
        {
            parserStore = new Dictionary<string, HttpHeaderParser>(HeaderUtilities.CaseInsensitiveStringComparer);

            parserStore.Add(HttpKnownHeaderNames.AcceptRanges, GenericHeaderParser.TokenListParser);
            parserStore.Add(HttpKnownHeaderNames.Age, TimeSpanHeaderParser.Parser);
            parserStore.Add(HttpKnownHeaderNames.ETag, GenericHeaderParser.SingleValueEntityTagParser);
            // The RFC says that the Location header should be an absolute Uri, 
            // but IIS and HttpListener do not enforce this.
            parserStore.Add(HttpKnownHeaderNames.Location, UriHeaderParser.RelativeOrAbsoluteUriParser);
            parserStore.Add(HttpKnownHeaderNames.ProxyAuthenticate, GenericHeaderParser.MultipleValueAuthenticationParser);
            parserStore.Add(HttpKnownHeaderNames.RetryAfter, GenericHeaderParser.RetryConditionParser);
            parserStore.Add(HttpKnownHeaderNames.Server, ProductInfoHeaderParser.Parser);
            parserStore.Add(HttpKnownHeaderNames.Vary, GenericHeaderParser.TokenListParser);
            parserStore.Add(HttpKnownHeaderNames.WWWAuthenticate, GenericHeaderParser.MultipleValueAuthenticationParser);

            HttpGeneralHeaders.AddParsers(parserStore);

            invalidHeaders = new HashSet<string>(HeaderUtilities.CaseInsensitiveStringComparer);
            HttpRequestHeaders.AddKnownHeaders(invalidHeaders);
            HttpContentHeaders.AddKnownHeaders(invalidHeaders);
        }

        internal static void AddKnownHeaders(HashSet<string> headerSet)
        {
            Contract.Requires(headerSet != null);

            headerSet.Add(HttpKnownHeaderNames.AcceptRanges);
            headerSet.Add(HttpKnownHeaderNames.Age);
            headerSet.Add(HttpKnownHeaderNames.ETag);
            headerSet.Add(HttpKnownHeaderNames.Location);
            headerSet.Add(HttpKnownHeaderNames.ProxyAuthenticate);
            headerSet.Add(HttpKnownHeaderNames.RetryAfter);
            headerSet.Add(HttpKnownHeaderNames.Server);
            headerSet.Add(HttpKnownHeaderNames.Vary);
            headerSet.Add(HttpKnownHeaderNames.WWWAuthenticate);
        }
    }
}
