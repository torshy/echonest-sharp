using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http.Headers;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Net.Http
{
    public class HttpClientChannel : HttpMessageChannel
    {
        #region Fields

        private static readonly Action<object> onCancel = OnCancel;
        private static readonly HashSet<string> knownContentHeaders;

        private readonly AsyncCallback getRequestStreamCallback;
        private readonly AsyncCallback getResponseCallback;

        private volatile bool operationStarted;
        private volatile bool disposed;

        private int maxRequestContentBufferSize;
        private CookieContainer cookieContainer;
        private bool useCookies;
        private DecompressionMethods automaticDecompression;
        private IWebProxy proxy;
        private bool useProxy;
        private X509CertificateCollection clientCertificates;
        private bool preAuthenticate;
        private bool useDefaultCredentials;
        private ICredentials credentials;
        private bool allowAutoRedirect;
        private int maxAutomaticRedirections;

        #endregion Fields

        #region Properties

        public virtual bool SupportsAutomaticDecompression
        {
            get { return true; }
        }

        public virtual bool SupportsProxy 
        {
            get { return true; }
        }

        public virtual bool SupportsClientCertificates
        {
            get { return true; }
        }

        public virtual bool SupportsRedirectConfiguration
        {
            get { return true; }
        }
        
        public bool UseCookies
        {
            get { return useCookies; }
            set
            {
                CheckDisposedOrStarted();
                useCookies = value;
            }
        }

        public CookieContainer CookieContainer
        {
            get { return cookieContainer; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                CheckDisposedOrStarted();
                cookieContainer = value;
            }
        }
        
        public DecompressionMethods AutomaticDecompression
        {
            get { return automaticDecompression; }
            set
            {
                CheckDisposedOrStarted();
                automaticDecompression = value;
            }
        }

        public bool UseProxy
        {
            get { return useProxy; }
            set
            {
                CheckDisposedOrStarted();
                useProxy = value;
            }
        }
        
        public IWebProxy Proxy
        {
            get { return proxy; }
            set
            {
                CheckDisposedOrStarted();
                proxy = value;
            }
        }

        public X509CertificateCollection ClientCertificates 
        {
            get
            {
                if (clientCertificates == null)
                {
                    clientCertificates = new X509CertificateCollection();
                }
                return clientCertificates;
            }
        }
        
        public bool PreAuthenticate
        {
            get { return preAuthenticate; }
            set
            {
                CheckDisposedOrStarted();
                preAuthenticate = value;
            }
        }
        
        public bool UseDefaultCredentials
        {
            get { return useDefaultCredentials; }
            set
            {
                CheckDisposedOrStarted();
                useDefaultCredentials = value;
            }
        }

        public ICredentials Credentials
        {
            get { return credentials; }
            set
            {
                CheckDisposedOrStarted();
                credentials = value;
            }
        }

        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            set
            {
                CheckDisposedOrStarted();
                allowAutoRedirect = value;
            }
        }

        public int MaxAutomaticRedirections
        {
            get { return maxAutomaticRedirections; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                CheckDisposedOrStarted();
                maxAutomaticRedirections = value;
            }
        }

        public int MaxRequestContentBufferSize
        {
            get { return maxRequestContentBufferSize; }
            set
            {
                // Setting the value to 0 is OK: It means the user doesn't want the channel to buffer content.
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                CheckDisposedOrStarted();
                maxRequestContentBufferSize = value;
            }
        }

        #endregion Properties

        #region De/Constructors

        static HttpClientChannel()
        {
            knownContentHeaders = new HashSet<string>(HeaderUtilities.CaseInsensitiveStringComparer);
            HttpContentHeaders.AddKnownHeaders(knownContentHeaders);
        }

        public HttpClientChannel()
        {
            this.getRequestStreamCallback = GetRequestStreamCallback;
            this.getResponseCallback = GetResponseCallback;

            // Set HWR default values
            this.allowAutoRedirect = true;
            this.maxRequestContentBufferSize = HttpContent.DefaultMaxBufferSize;
            this.automaticDecompression = DecompressionMethods.None;
            this.clientCertificates = null; // only create collection when required.
            this.cookieContainer = new CookieContainer(); // default container used for dealing with auto-cookies.
            this.credentials = null;
            this.maxAutomaticRedirections = 50;
            this.preAuthenticate = false;
            this.proxy = null;
            this.useProxy = true;
            this.useCookies = true; // deal with cookies by default.
            this.useDefaultCredentials = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;
            }
            base.Dispose(disposing);
        }

        #endregion De/Constructors

        #region Request Setup

        private HttpWebRequest CreateAndPrepareWebRequest(HttpRequestMessage request)
        {
            HttpWebRequest webRequest = WebRequest.Create(request.RequestUri) as HttpWebRequest;
            webRequest.AllowWriteStreamBuffering = true;

            webRequest.Method = request.Method.Method;
            webRequest.ProtocolVersion = request.Version;

            SetDefaultOptions(webRequest);
            SetConnectionOptions(webRequest, request);
            SetServicePointOptions(webRequest, request);
            SetRequestHeaders(webRequest, request);
            SetContentHeaders(webRequest, request);
            PrepareWebRequestForContentUpload(webRequest, request);
            // For Extensibility
            InitializeWebRequest(request, webRequest);

            return webRequest;
        }

        // Needs to be internal so that WebRequestChannel can access it from a different assembly.
        internal virtual void InitializeWebRequest(HttpRequestMessage request, HttpWebRequest webRequest)
        {
        }

        private void SetDefaultOptions(HttpWebRequest webRequest)
        {
            webRequest.Timeout = Timeout.Infinite; // Timeouts are handled by HttpClient.

            webRequest.AllowAutoRedirect = allowAutoRedirect;
            webRequest.AutomaticDecompression = automaticDecompression;
            webRequest.PreAuthenticate = preAuthenticate;

            if (useDefaultCredentials)
            {
                webRequest.UseDefaultCredentials = true;
            }
            else
            {
                webRequest.Credentials = credentials;
            }

            if (allowAutoRedirect)
            {
                webRequest.MaximumAutomaticRedirections = maxAutomaticRedirections;
            }

            if (useProxy)
            {
                // If 'UseProxy' is true and 'Proxy' is null (default), let HWR figure out the proxy to use. Otherwise
                // set the custom proxy.
                if (proxy != null)
                {
                    webRequest.Proxy = proxy;
                }
            }
            else
            {
                // The use explicitly specified to not use a proxy. Set HWR.Proxy to null to make sure HWR doesn't use
                // a proxy for this request.
                webRequest.Proxy = null;
            }

            if ((clientCertificates != null) && (clientCertificates.Count > 0))
            {
                webRequest.ClientCertificates = clientCertificates;
            }

            if (useCookies)
            {
                webRequest.CookieContainer = cookieContainer; 
            }
        }

        private static void SetConnectionOptions(HttpWebRequest webRequest, HttpRequestMessage request)
        {
            if (request.Version <= HttpVersion.Version10)
            {
                // HTTP 1.0 had some support for persistent connections by allowing "Connection: Keep-Alive". Check
                // whether this value is set.
                bool keepAliveSet = false;
                foreach (string item in request.Headers.Connection)
                {
                    if (string.Compare(item, "Keep-Alive", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        keepAliveSet = true;
                        break;
                    }
                }
                webRequest.KeepAlive = keepAliveSet;
            }
            else
            {
                // HTTP 1.1 uses persistent connections by default. If the user doesn't want to use persistent 
                // connections, he can set 'ConnectionClose' to true (equivalent to header "Connection: close").
                if (request.Headers.ConnectionClose == true)
                {
                    webRequest.KeepAlive = false;
                }
            }
        }

        private void SetServicePointOptions(HttpWebRequest webRequest, HttpRequestMessage request)
        {
            HttpRequestHeaders headers = request.Headers;
            ServicePoint currentServicePoint = null;

            // We have to update the ServicePoint in order to support "Expect: 100-continue". This setting may affect
            // also requests sent by other HWR instances (or HttpClient instances). This is a known limitation.
            bool? expectContinue = headers.ExpectContinue;
            if (expectContinue != null)
            {
                currentServicePoint = webRequest.ServicePoint;
                currentServicePoint.Expect100Continue = (bool)expectContinue;
            }
        }

        private static void SetRequestHeaders(HttpWebRequest webRequest, HttpRequestMessage request)
        {
            WebHeaderCollection webRequestHeaders = webRequest.Headers;
            HttpRequestHeaders headers = request.Headers;

            // Most headers are just added directly to HWR's internal headers collection. But there are some exceptions
            // requiring different handling.
            // The following bool vars are used to skip string comparison when not required: E.g. if the 'Host' header
            // was not set, we don't need to compare every header in the collection with 'Host' to make sure we don't
            // add it to HWR's header collection.
            bool isHostSet = headers.Contains(HttpKnownHeaderNames.Host);
            bool isExpectSet = headers.Contains(HttpKnownHeaderNames.Expect);
            bool isTransferEncodingSet = headers.Contains(HttpKnownHeaderNames.TransferEncoding);
            bool isConnectionSet = headers.Contains(HttpKnownHeaderNames.Connection);
            bool isAcceptSet = headers.Contains(HttpKnownHeaderNames.Accept);
            bool isDateSet = headers.Contains(HttpKnownHeaderNames.Date);
            bool isIfModifiedSinceSet = headers.Contains(HttpKnownHeaderNames.IfModifiedSince);
            bool isRangeSet = headers.Contains(HttpKnownHeaderNames.Range);
            bool isRefererSet = headers.Contains(HttpKnownHeaderNames.Referer);
            bool isUserAgentSet = headers.Contains(HttpKnownHeaderNames.UserAgent);

            if (isHostSet)
            {
                string host = headers.Host;
                if (host != null)
                {
                    webRequest.Host = host;
                }
            }

            if (isDateSet)
            {
                DateTimeOffset? date = headers.Date;
                if (date != null)
                {
                    webRequest.Date = date.Value.Date;
                }
            }

            if (isIfModifiedSinceSet)
            {
                DateTimeOffset? ifModifiedSince = headers.IfModifiedSince;
                if (ifModifiedSince != null)
                {
                    webRequest.IfModifiedSince = ifModifiedSince.Value.Date;
                }
            }

            if (isRangeSet)
            {
                RangeHeaderValue range = headers.Range;
                if (range != null)
                {
                    foreach(var rangeItem in range.Ranges)
                    {
                        webRequest.AddRange((long) rangeItem.From, (long) rangeItem.To);
                    }
                }
            }

            if (isRefererSet)
            {
                Uri referer = headers.Referrer;
                if (referer != null)
                {
                    webRequest.Referer = referer.OriginalString;
                }
            }


            // The following headers (Expect, Transfer-Encoding, Connection) have both a collection property and a 
            // bool property indicating a special value. Internally (in HttpHeaders) we don't distinguish between 
            // "special" values and other values. So we must make sure that we add all but the special value to HWR.
            // E.g. the 'Transfer-Encoding: chunked' value must be set using HWR.SendChunked, whereas all other values
            // can be added to the 'Transfer-Encoding'. The collection property (headers.Expect, headers.Connection,
            // headers.TransferEncoding) only return non-special values.
            if (isExpectSet && (headers.Expect.Count > 0))
            {
                webRequest.Expect = GetValueString(headers.Expect);
            }

            if (isTransferEncodingSet && (headers.TransferEncoding.Count > 0))
            {
                webRequest.TransferEncoding = GetValueString(headers.TransferEncoding);
            }

            if (isConnectionSet && (headers.Connection.Count > 0))
            {
                webRequest.Connection = GetValueString(headers.Connection);
            }


            if (isAcceptSet && (headers.Accept.Count > 0))
            {
                webRequest.Accept = GetValueString(headers.Accept);
            }

            if (isUserAgentSet && headers.UserAgent.Count > 0)
            {
                webRequest.UserAgent = GetValueString(headers.UserAgent);
            }

            foreach (var header in request.Headers.GetHeaderStrings())
            {
                string headerName = header.Key;

                if ((isHostSet && AreEqual(HttpKnownHeaderNames.Host, headerName)) ||
                    (isExpectSet && AreEqual(HttpKnownHeaderNames.Expect, headerName)) ||
                    (isTransferEncodingSet && AreEqual(HttpKnownHeaderNames.TransferEncoding, headerName)) ||
                    (isConnectionSet && AreEqual(HttpKnownHeaderNames.Connection, headerName)) ||
                    (isAcceptSet && AreEqual(HttpKnownHeaderNames.Accept, headerName)) ||
                    (isDateSet && AreEqual(HttpKnownHeaderNames.Date, headerName)) ||
                    (isIfModifiedSinceSet && AreEqual(HttpKnownHeaderNames.IfModifiedSince, headerName)) ||
                    (isRangeSet && AreEqual(HttpKnownHeaderNames.Range, headerName)) ||
                    (isRefererSet && AreEqual(HttpKnownHeaderNames.Referer, headerName)) ||
                    (isUserAgentSet) && AreEqual(HttpKnownHeaderNames.UserAgent, headerName))
                {
                    continue; // Header was already added.
                }
                                
                webRequestHeaders.Add(header.Key, header.Value);
            }
        }

        private static void SetContentHeaders(HttpWebRequest webRequest, HttpRequestMessage request)
        {
            if (request.Content != null)
            {
                HttpContentHeaders headers = request.Content.Headers;

                // All content headers besides Content-Length can be added directly to HWR. So just check whether we 
                // have the Content-Length header set. If not, add all headers, otherwise skip the Content-Length 
                // header.
                // Note that this method is called _before_ PrepareWebRequestForContentUpload(): I.e. in most scenarios
                // this means that no one accessed Headers.ContentLength property yet, thus there will be no 
                // Content-Length header in the store. I.e. we'll end up in the 'else' block providing better perf, 
                // since no string comparison is required.
                if (headers.Contains(HttpKnownHeaderNames.ContentLength))
                {
                    foreach (var header in request.Content.Headers)
                    {
                        if (string.CompareOrdinal(HttpKnownHeaderNames.ContentType, header.Key) == 0)
                        {
                            webRequest.ContentType = string.Join(", ", header.Value);
                            continue;
                        }

                        if (string.CompareOrdinal(HttpKnownHeaderNames.ContentLength, header.Key) != 0)
                        {
                            webRequest.Headers.Add(header.Key, string.Join(", ", header.Value));
                        }
                    }
                }
                else
                {
                    foreach (var header in request.Content.Headers)
                    {
                        if (string.CompareOrdinal(HttpKnownHeaderNames.ContentLength, header.Key) == 0)
                        {
                            continue; 
                        }

                        if (string.CompareOrdinal(HttpKnownHeaderNames.ContentType, header.Key) == 0)
                        {
                            webRequest.ContentType = string.Join(", ", header.Value);
                            continue;
                        }
                        
                        webRequest.Headers.Add(header.Key, string.Join(", ", header.Value));
                    }
                }
            }
        }

        private void PrepareWebRequestForContentUpload(HttpWebRequest webRequest, HttpRequestMessage request)
        {
            HttpContent requestContent = request.Content;
            if (requestContent == null)
            {
                // If we have no content, we set the Content-Length to 0, regardless of the Transfer-Encoding header.
                webRequest.ContentLength = 0;
            }
            else
            {
                MediaTypeHeaderValue contentType = requestContent.Headers.ContentType;
                if (contentType != null)
                {
                    webRequest.ContentType = contentType.ToString();
                }

                // Determine how to communicate the length of the request content.
                if (request.Headers.TransferEncodingChunked == true)
                {
                    webRequest.SendChunked = true;
                }
                else
                {
                    long? contentLength = requestContent.Headers.ContentLength;
                    if (contentLength != null)
                    {
                        webRequest.ContentLength = (long)contentLength;
                    }
                    else
                    {
                        // If we don't have a content length and we don't use chunked, then we must buffer the content.
                        // If the user specified a zero buffer size, we throw.
                        if (maxRequestContentBufferSize == 0)
                        {
                            throw new HttpException(
                                "The content length of the request content can't be determined. Either set TransferEncodingChunked to true, load content into buffer, or set AllowRequestContentBuffering to true.");
                        }

                        // HttpContent couldn't calculate the content length. Chunked is not specified. Buffer the 
                        // content to get the content length.
                        // Note that we call LoadIntoBuffer() synchronously since the scenario where neither the length
                        // is known nor chunked is used, often means that we're about to serialize in-memory data like
                        // XML, objects, feed content, etc. I.e. loading sync results in better perf. than async.
                        // If customers want to use async buffering (e.g. the source is a network stream with
                        // unknown length), LoadIntoBufferAsync() can be called before sending the request. In this
                        // case the length is known and we'll not end up here.
                        requestContent.LoadIntoBuffer(maxRequestContentBufferSize);
                        contentLength = requestContent.Headers.ContentLength;
                        Contract.Assert(contentLength != null, "After buffering content, ContentLength must not be null.");
                        webRequest.ContentLength = (long)contentLength;
                    }
                }
            }
        }

        #endregion Message Setup

        #region Request Processing

        protected internal override HttpResponseMessage Send(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "A request message must be provided. It cannot be null.");
            }
            CheckDisposed();

            SetOperationStarted();

            HttpResponseMessage response = null;
            try
            {
                HttpWebRequest webRequest = CreateAndPrepareWebRequest(request);
                cancellationToken.Register(onCancel, webRequest);

                if (request.Content != null)
                {
                    // Note that we don't check whether the current method allows request bodies. This is done in HWR.
                    // We're trying to avoid double-checking where possible.
                    TransportContext context = null;
                    Stream requestStream = webRequest.GetRequestStream(out context);
                    request.Content.CopyTo(requestStream, context);
                    requestStream.Close();
                }

                HttpWebResponse webResponse = null;
                try
                {
                    webResponse = webRequest.GetResponse() as HttpWebResponse;
                }
                catch (WebException e)
                {
                    if (e.Response == null)
                    {
                        throw;
                    }

                    webResponse = e.Response as HttpWebResponse;
                }

                response = CreateResponseMessage(webResponse, request);
                return response;
            }
            catch (WebException e)
            {
                // If the WebException was due to the cancellation token being canceled, throw cancellation exception.
                cancellationToken.ThrowIfCancellationRequested();

                // If the WebException wasn't due to cancellation, then wrap it in an HttpException and return.
                throw new HttpException("An error occurred while sending the request.", e);
            }
        }

        protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "A request message must be provided. It cannot be null.");
            }
            CheckDisposed();

            SetOperationStarted();

            TaskCompletionSource<HttpResponseMessage> tcs = new TaskCompletionSource<HttpResponseMessage>();
            try
            {
                // Cancellation: Note that there is no race here: If the token gets canceled before we register the
                // callback, the token will invoke the callback immediately. I.e. HWR gets aborted before we use it.
                HttpWebRequest webRequest = CreateAndPrepareWebRequest(request);
                cancellationToken.Register(onCancel, webRequest);

                RequestState state = new RequestState();
                state.webRequest = webRequest;
                state.tcs = tcs;
                state.cancellationToken = cancellationToken;
                state.requestMessage = request;
                if (request.Content == null)
                {
                    webRequest.BeginGetResponse(getResponseCallback, state);
                }
                else
                {
                    webRequest.BeginGetRequestStream(getRequestStreamCallback, state);
                }
            }
            catch (Exception e)
            {
                // Wrap WebException as HttpExceptions since this is considered an error during execution. All other
                // exception types are 'unexpected' and should not be wrapped.
                HandleException(tcs, cancellationToken, e is WebException ?
                    new HttpException("An error occurred while sending the request.", e) : e);
            }

            return tcs.Task;
        }

        private void GetRequestStreamCallback(IAsyncResult ar)
        {
            RequestState state = ar.AsyncState as RequestState;
            Contract.Assert(state != null);

            try
            {
                TransportContext context = null;
                Stream requestStream = state.webRequest.EndGetRequestStream(ar, out context) as Stream;
                state.requestStream = requestStream;
                state.requestMessage.Content.CopyToAsync(requestStream, context).ContinueWith(task =>
                {
                    try
                    {
                        if (task.IsFaulted)
                        {
                            HandleException(state.tcs, state.cancellationToken, task.Exception.GetBaseException());
                            return;
                        }

                        if (task.IsCanceled)
                        {
                            state.tcs.TrySetCanceled();
                            return;
                        }

                        state.requestStream.Close();
                        state.webRequest.BeginGetResponse(getResponseCallback, state);
                    }
                    catch (Exception e)
                    {
                        HandleException(state.tcs, state.cancellationToken, e);
                    }

                }, TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception e)
            {
                HandleException(state.tcs, state.cancellationToken, e);
            }
        }

        private void GetResponseCallback(IAsyncResult ar)
        {
            RequestState state = ar.AsyncState as RequestState;
            Contract.Assert(state != null);

            try
            {
                HttpWebResponse webResponse = state.webRequest.EndGetResponse(ar) as HttpWebResponse;
                state.tcs.TrySetResult(CreateResponseMessage(webResponse, state.requestMessage));
            }
            catch (Exception e)
            {
                HandleException(state.tcs, state.cancellationToken, e);
            }
        }

        private HttpResponseMessage CreateResponseMessage(HttpWebResponse webResponse, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(webResponse.StatusCode,
                webResponse.StatusDescription);
            response.Version = webResponse.ProtocolVersion;
            response.RequestMessage = request;
            response.Content = new StreamContent(webResponse.GetResponseStream());

            // Update Request-URI to reflect the URI actually leading to the response message.
            request.RequestUri = webResponse.ResponseUri;

            WebHeaderCollection webResponseHeaders = webResponse.Headers;
            HttpContentHeaders contentHeaders = response.Content.Headers;
            HttpResponseHeaders responseHeaders = response.Headers;

            // HttpWebResponse.ContentLength is set to -1 if no Content-Length header is provided.
            if (webResponse.ContentLength >= 0)
            {
                contentHeaders.ContentLength = webResponse.ContentLength;
            }

            for (int i = 0; i < webResponseHeaders.Count; i++)
            {
                string currentHeader = webResponseHeaders.GetKey(i);

                if (knownContentHeaders.Contains(currentHeader))
                {
                    // We already set Content-Length
                    if (string.Compare(currentHeader, HttpKnownHeaderNames.ContentLength,
                        StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        continue;
                    }

                    AddHeaderValues(webResponseHeaders, i, currentHeader, contentHeaders);
                }
                else
                {
                    AddHeaderValues(webResponseHeaders, i, currentHeader, responseHeaders);
                }
            }

            return response;
        }

        private void HandleException(TaskCompletionSource<HttpResponseMessage> tcs,
            CancellationToken cancellationToken, Exception e)
        {
            // If the WebException was due to the cancellation token being canceled, throw cancellation exception.
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled();
            }
            else
            {
                // Use 'SendAsync' as method name, since this method is only called by methods in the async code path. Using
                // 'SendAsync' as method name helps relate the exception to the operation in log files.

                if (e is HttpException)
                {
                    tcs.TrySetException(e);
                }
                else
                {
                    tcs.TrySetException(new HttpException("An error occurred while sending the request.", e));
                }
            }
        }

        private static void OnCancel(object state)
        {
            HttpWebRequest webRequest = state as HttpWebRequest;
            Contract.Assert(webRequest != null);

            webRequest.Abort();
        }
        
        #endregion Request Processing

        #region Helpers

        private void SetOperationStarted()
        {
            if (!operationStarted)
            {
                operationStarted = true;
            }
        }

        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        internal void CheckDisposedOrStarted()
        {
            CheckDisposed();
            if (operationStarted)
            {
                throw new InvalidOperationException("The HttpClient instance already started one or more requests. Properties can only be modified before sending the first request.");
            }
        }

        private static void AddHeaderValues(WebHeaderCollection source, int index, string header, HttpHeaders destination)
        {
            string[] values = source.GetValues(index);

            // Even though AddWithoutValidation() could throw FormatException for header values containing newline
            // chars that are not followed by whitespace chars, we don't need to catch that exception. Our header
            // values were returned by HttpWebResponse which is trusted to only return valid header values.
            if (values.Length == 1)
            {
                destination.AddWithoutValidation(header, values[0]);
            }
            else
            {
                for (int j = 0; j < values.Length; j++)
                {
                    destination.AddWithoutValidation(header, values[j]);
                }
            }
        }

        private static bool AreEqual(string x, string y)
        {
            return (string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static string GetValueString(System.Collections.IEnumerable collection)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var expectValue in collection)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(expectValue.ToString());
            }
            return sb.ToString();
        }

        #endregion Helpers

        private class RequestState
        {
            internal HttpWebRequest webRequest;
            internal TaskCompletionSource<HttpResponseMessage> tcs;
            internal CancellationToken cancellationToken;
            internal HttpRequestMessage requestMessage;
            internal Stream requestStream;
        }
    }
}
