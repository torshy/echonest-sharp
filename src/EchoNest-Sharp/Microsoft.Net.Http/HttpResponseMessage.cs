using System.Net.Http.Headers;
using System.Text;

namespace System.Net.Http
{
    public class HttpResponseMessage : IDisposable
    {
        private const HttpStatusCode defaultStatusCode = HttpStatusCode.OK;
        private const string defaultReasonPhrase = "OK";

        private HttpStatusCode statusCode;
        private HttpResponseHeaders headers;
        private string reasonPhrase;
        private HttpRequestMessage requestMessage;
        private Version version;
        private HttpContent content;
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
                content = value; 
            } 
        }

        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
            set 
            {
                if (((int)statusCode < 0) || ((int)statusCode > 999))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                CheckDisposed();

                statusCode = value; 
            }
        }

        public string ReasonPhrase
        {
            get { return reasonPhrase; }
            set
            {
                if ((value != null) && ContainsNewLineCharacter(value))
                {
                    throw new FormatException("The reason phrase must not contain new-line characters.");
                }
                CheckDisposed();

                reasonPhrase = value; // It's OK to have a 'null' reason phrase
            }
        }

        public HttpResponseHeaders Headers
        {
            get
            {
                if (headers == null)
                {
                    headers = new HttpResponseHeaders();
                }
                return headers;
            }
        }

        public HttpRequestMessage RequestMessage
        {
            get { return requestMessage; }
            set 
            {
                CheckDisposed();
                requestMessage = value; 
            }
        }

        public bool IsSuccessStatusCode
        {
            get { return ((int)statusCode >= 200) && ((int)statusCode <= 299); }
        }

        public HttpResponseMessage()
            : this(defaultStatusCode, defaultReasonPhrase)
        {
        }

        public HttpResponseMessage(HttpStatusCode statusCode, string reasonPhrase)
        {
            if (((int)statusCode < 0) || ((int)statusCode > 999))
            {
                throw new ArgumentOutOfRangeException("statusCode");
            }

            this.statusCode = statusCode;
            this.reasonPhrase = reasonPhrase;
            this.version = HttpUtilities.DefaultVersion;
        }

        public HttpResponseMessage EnsureSuccessStatusCode()
        {
            if (!IsSuccessStatusCode)
            {
                // Disposing the content should help users: If users call EnsureSuccessStatusCode(), an exception is
                // thrown if the response status code is != 2xx. I.e. the behavior is similar to a failed request (e.g.
                // connection failure). Users don't expect to dispose the content in this case: If an exception is 
                // thrown, the object is responsible fore cleaning up its state.
                if (content != null)
                {
                    content.Dispose();
                }

                throw new HttpException(string.Format("Response status code does not indicate success: {0} ({1})", (int)statusCode,
                    reasonPhrase));
            }
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("StatusCode: ");
            sb.Append((int)statusCode);

            sb.Append(", ReasonPhrase: '");
            sb.Append(reasonPhrase == null ? "<null>" : reasonPhrase);

            sb.Append("', Version: ");
            sb.Append(version);

            sb.Append(", Content: ");
            sb.Append(content == null ? "<null>" : content.GetType().FullName);

            sb.Append(", Headers:\r\n");
            sb.Append(HeaderUtilities.DumpHeaders(headers, content == null ? null : content.Headers));

            return sb.ToString();
        }

        private bool ContainsNewLineCharacter(string value)
        {
            foreach (char character in value)
            {
                if ((character == HttpRuleParser.CR) || (character == HttpRuleParser.LF))
                {
                    return true;
                }
            }
            return false;
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
