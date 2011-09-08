using System.Net.Cache;
using System.Net.Security;
using System.Security.Principal;

namespace System.Net.Http
{
    public class WebRequestChannel : HttpClientChannel
    {
        #region Fields

        private AuthenticationLevel authenticationLevel;
        private TokenImpersonationLevel impersonationLevel;
        private bool allowPipelining;
        private bool unsafeAuthenticatedConnectionSharing;
        private int maxResponseHeadersLength;
        private int readWriteTimeout;
        private RequestCachePolicy cachePolicy;

        #endregion Fields

        #region Properties

        public AuthenticationLevel AuthenticationLevel
        {
            get { return authenticationLevel; }
            set
            {
                CheckDisposedOrStarted();
                authenticationLevel = value;
            }
        }

        public TokenImpersonationLevel ImpersonationLevel
        {
            get { return impersonationLevel; }
            set
            {
                CheckDisposedOrStarted();
                impersonationLevel = value;
            }
        }

        public bool AllowPipelining
        {
            get { return allowPipelining; }
            set
            {
                CheckDisposedOrStarted();
                allowPipelining = value;
            }
        }

        public bool UnsafeAuthenticatedConnectionSharing
        {
            get { return unsafeAuthenticatedConnectionSharing; }
            set
            {
                CheckDisposedOrStarted();
                unsafeAuthenticatedConnectionSharing = value;
            }
        }

        public int MaxResponseHeadersLength
        {
            get { return maxResponseHeadersLength; }
            set
            {
                if (value <= 0)
                {
                    // This custom error message exists primarily to make sure localization works in this assembly.
                    // There are currently no other resource strings.
                    throw new ArgumentOutOfRangeException("value", "The input must be greater than zero.");
                }
                CheckDisposedOrStarted();
                maxResponseHeadersLength = value;
            }
        }

        public int ReadWriteTimeout
        {
            get { return readWriteTimeout; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                CheckDisposedOrStarted();
                readWriteTimeout = value;
            }
        }

        public RequestCachePolicy CachePolicy
        {
            get { return cachePolicy; }
            set
            {
                CheckDisposedOrStarted();
                cachePolicy = value;
            }
        }

        #endregion Properties

        #region Constructor

        public WebRequestChannel()
        {
            // Set HWR default values
            this.allowPipelining = true;
            this.authenticationLevel = AuthenticationLevel.MutualAuthRequested;
            this.cachePolicy = WebRequest.DefaultCachePolicy;
            this.impersonationLevel = TokenImpersonationLevel.Delegation;
            this.maxResponseHeadersLength = HttpWebRequest.DefaultMaximumResponseHeadersLength;
            this.readWriteTimeout = 5 * 60 * 1000; // 5 minutes
            this.unsafeAuthenticatedConnectionSharing = false;
        }

        #endregion Constructor

        #region Request Setup

        internal override void InitializeWebRequest(HttpRequestMessage request, HttpWebRequest webRequest)
        {
            // WebRequestChannel specific properties
            webRequest.AuthenticationLevel = authenticationLevel;
            webRequest.CachePolicy = cachePolicy;
            webRequest.ImpersonationLevel = impersonationLevel;
            webRequest.MaximumResponseHeadersLength = maxResponseHeadersLength;
            webRequest.Pipelined = allowPipelining;
            webRequest.ReadWriteTimeout = readWriteTimeout;
            webRequest.UnsafeAuthenticatedConnectionSharing = unsafeAuthenticatedConnectionSharing;
        }

        #endregion Request Setup
    }
}