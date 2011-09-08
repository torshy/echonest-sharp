using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace System.Net.Http
{
    public class HttpRequestMessage : IDisposable
    {
        private const int messageAlreadySent = 1; // signals that this message was already sent. 
        private const int messageNotYetSent = 0;

        // If this field is 0 (default), then the message wasn't sent by an HttpClient instance yet. If the field
        // value is 'messageSent', then the message was already sent and should not be sent again.
        private int sendStatus;

        private HttpMethod method;
        private Uri requestUri;
        private HttpRequestHeaders headers;
        private Version version;
        private HttpContent content;
        private IDictionary<string, object> properties;
        private bool disposed;

        public Version Version
        {
            get { return version; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                CheckDisposed();

                version = value;
            }
        }

        public HttpContent Content
        {
            get { return content; }
            set
            {
                CheckDisposed();

                // It's OK to set a 'null' content, even if the method is POST/PUT. We don't want to artificially
                // prevent scenarios by being to strict.
                content = value;
            }
        }

        public HttpMethod Method
        {
            get { return method; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                CheckDisposed();

                method = value;
            }
        }

        public IDictionary<string, object> Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }
                return this.properties;
            }
        }

        public Uri RequestUri
        {
            get { return requestUri; }
            set
            {
                if ((value != null) && (value.IsAbsoluteUri) && (!HttpUtilities.IsHttpUri(value)))
                {
                    throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", "value");
                }
                CheckDisposed();

                // It's OK to set 'null'. HttpClient will add the 'BaseAddress'. If there is no 'BaseAddress'
                // sending this message will throw.
                requestUri = value;
            }
        }

        public HttpRequestHeaders Headers
        {
            get
            {
                if (headers == null)
                {
                    headers = new HttpRequestHeaders();
                }
                return headers;
            }
        }

        public HttpRequestMessage()
            : this(HttpMethod.Get, (Uri)null)
        {
        }

        public HttpRequestMessage(HttpMethod method, Uri requestUri)
        {
            InitializeValues(method, requestUri);
        }

        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads",
            Justification = "It is OK to provide 'null' values. A Uri instance is created from 'requestUri' if it is != null.")]
        public HttpRequestMessage(HttpMethod method, string requestUri)
        {
            // It's OK to have a 'null' request Uri. If HttpClient is used, the 'BaseAddress' will be added.
            // If there is no 'BaseAddress', sending this request message will throw.
            // Note that we also allow the string to be empty: null and empty should be considered equivalent.
            if (string.IsNullOrEmpty(requestUri))
            {
                InitializeValues(method, null);
            }
            else
            {
                InitializeValues(method, new Uri(requestUri, UriKind.RelativeOrAbsolute));
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Method: ");
            sb.Append(method);

            sb.Append(", RequestUri: '");
            sb.Append(requestUri == null ? "<null>" : requestUri.ToString());

            sb.Append("', Version: ");
            sb.Append(version);

            sb.Append(", Content: ");
            sb.Append(content == null ? "<null>" : content.GetType().FullName);

            sb.Append(", Headers:\r\n");
            sb.Append(HeaderUtilities.DumpHeaders(headers, content == null ? null : content.Headers));

            return sb.ToString();
        }

        private void InitializeValues(HttpMethod method, Uri requestUri)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            if ((requestUri != null) && (requestUri.IsAbsoluteUri) && (!HttpUtilities.IsHttpUri(requestUri)))
            {
                throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", "requestUri");
            }

            this.method = method;
            this.requestUri = requestUri;
            this.version = HttpUtilities.DefaultVersion;
        }

        internal bool MarkAsSent()
        {
            return Interlocked.Exchange(ref sendStatus, messageAlreadySent) == messageNotYetSent;
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            // The reason for this type to implement IDisposable is that it contains instances of types that implement
            // IDisposable (content). 
            if (disposing && !disposed)
            {
                disposed = true;
                if (content != null)
                {
                    content.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
