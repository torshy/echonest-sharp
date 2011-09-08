using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace System.Net.Http
{
    public class HttpClient : IDisposable
    {
        private static readonly TimeSpan defaultTimeout = new TimeSpan(0, 1, 40);
        private const HttpCompletionOption defaultCompletionOption = HttpCompletionOption.ResponseContentRead;

        private volatile bool operationStarted;
        private volatile bool disposed;

        private CancellationTokenSource pendingRequestsCts;
        private HttpMessageChannel actualChannel;
        private HttpMessageChannel userChannel;
        private HttpRequestHeaders defaultRequestHeaders;

        private Uri baseAddress;
        private TimeSpan timeout;
        private int maxResponseContentBufferSize;

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                if (defaultRequestHeaders == null)
                {
                    defaultRequestHeaders = new HttpRequestHeaders();
                }
                return defaultRequestHeaders;
            }
        }

        public Uri BaseAddress
        {
            get { return baseAddress; }
            set
            {
                CheckBaseAddress(value, "value");
                CheckDisposedOrStarted();

                baseAddress = value;
            }
        }

        public TimeSpan Timeout
        {
            get
            {
                // TODO not implemented
                throw new NotImplementedException("M2 task");
            }
            set
            {
                // TODO not implemented
                throw new NotImplementedException("M2 task");
            }
        }

        public int MaxResponseContentBufferSize
        {
            get { return maxResponseContentBufferSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                CheckDisposedOrStarted();
                maxResponseContentBufferSize = value;
            }
        }

        public HttpMessageChannel Channel
        {
            get { return userChannel; }
            set
            {
                CheckDisposedOrStarted();
                userChannel = value;
            }
        }

        private HttpMessageChannel ActualChannel
        {
            get
            {
                if (actualChannel != null)
                {
                    return actualChannel;
                }

                if (userChannel == null)
                {
                    actualChannel = new HttpClientChannel(); // create default channel.
                }
                else
                {
                    actualChannel = userChannel;
                }

                return actualChannel;
            }
        }

        public HttpClient()
            : this((Uri)null)
        {
        }

        public HttpClient(Uri baseAddress)
        {
            InitializeValues(baseAddress);
        }

        [SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads",
            Justification = "It is OK to provide 'null' values. A Uri instance is created from 'baseAddress' if it is != null.")]
        public HttpClient(string baseAddress)
        {
            // Note that we also allow the string to be empty: null and empty should be considered equivalent.
            if (string.IsNullOrEmpty(baseAddress))
            {
                InitializeValues(null);
            }
            else
            {
                // We could use UriKind.Absolute here, since we demand an absolute Uri. However, if the value is a
                // relative Uri, then Uri..ctor would throw UriFormatException. This way we create the Uri and check
                // if it is an absolute Uri. If not, we throw an ArgumentException.
                InitializeValues(new Uri(baseAddress, UriKind.RelativeOrAbsolute));
            }
        }

        public HttpResponseMessage Send(HttpRequestMessage request)
        {
            return Send(request, defaultCompletionOption, CancellationToken.None);
        }

        public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return Send(request, completionOption, CancellationToken.None);
        }

        public HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Send(request, defaultCompletionOption, cancellationToken);
        }

        public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            CheckDisposed();
            CheckRequestMessage(request);

            SetOperationStarted();
            PrepareRequestMessage(request);

            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,
                pendingRequestsCts.Token);
            SetTimeout(linkedCts);

            HttpResponseMessage response = null;
            try
            {
                response = ActualChannel.Send(request, linkedCts.Token);
                if (response == null)
                {
                    throw new InvalidOperationException("Channel did not return a response message.");
                }

                if ((completionOption == HttpCompletionOption.ResponseContentRead) && (response.Content != null))
                {
                    try
                    {
                        response.Content.LoadIntoBuffer(maxResponseContentBufferSize);
                    }
                    catch (HttpException)
                    {
                        // If the cancellation token was canceled, we consider the exception to be caused by the
                        // cancellation (e.g. WebException when reading from canceled response stream).
                        linkedCts.Token.ThrowIfCancellationRequested();
                        throw;
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                LogSendError(request, linkedCts, "Send", e);
                throw;
            }
            finally
            {
                DisposeRequestContent(request);
            }
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendAsync(request, defaultCompletionOption, CancellationToken.None);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return SendAsync(request, defaultCompletionOption, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return SendAsync(request, completionOption, CancellationToken.None);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            CheckDisposed();
            CheckRequestMessage(request);

            SetOperationStarted();
            PrepareRequestMessage(request);

            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,
                pendingRequestsCts.Token);
            SetTimeout(linkedCts);

            TaskCompletionSource<HttpResponseMessage> tcs = new TaskCompletionSource<HttpResponseMessage>();
            ActualChannel.SendAsync(request, linkedCts.Token).ContinueWith(task =>
            {
                try
                {
                    // The request is completed. Dispose the request content.
                    DisposeRequestContent(request);

                    if (task.IsFaulted)
                    {
                        SetTaskFaulted(request, linkedCts, tcs, task.Exception.GetBaseException());
                        return;
                    }

                    if (task.IsCanceled)
                    {
                        SetTaskCanceled(request, linkedCts, tcs);
                        return;
                    }

                    HttpResponseMessage response = task.Result;
                    if (response == null)
                    {
                        SetTaskFaulted(request, linkedCts, tcs,
                            new InvalidOperationException("Channel did not return a response message."));
                        return;
                    }

                    // If we don't have a response content, just return the response message.
                    if ((response.Content == null) || (completionOption == HttpCompletionOption.ResponseHeadersRead))
                    {
                        SetTaskCompleted(request, linkedCts, tcs, response);
                        return;
                    }
                    Contract.Assert(completionOption == HttpCompletionOption.ResponseContentRead,
                        "Unknown completion option.");

                    // We have an assigned content. Start loading it into a buffer and return response message once
                    // the whole content is buffered.
                    StartContentBuffering(request, linkedCts, tcs, response);
                }
                catch (Exception e)
                {
                    // Make sure we catch any exception, otherwise the task will catch it and throw in the finalizer.
                    tcs.TrySetException(e);
                }

            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public void CancelPendingRequests()
        {
            CheckDisposed();

            // With every request we link this cancellation token source.
            CancellationTokenSource currentCts = Interlocked.Exchange(ref pendingRequestsCts,
                new CancellationTokenSource());

            currentCts.Cancel();

        }

        public HttpResponseMessage Get(Uri requestUri)
        {
            return Send(new HttpRequestMessage(HttpMethod.Get, requestUri));
        }

        public HttpResponseMessage Get(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return Get((Uri)null);
            }
            return Get(new Uri(requestUri, UriKind.RelativeOrAbsolute));
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri));
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return GetAsync((Uri)null);
            }
            return GetAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute));
        }

        public HttpResponseMessage Post(Uri requestUri, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;
            return Send(request);
        }

        public HttpResponseMessage Post(string requestUri, HttpContent content)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return Post((Uri)null, content);
            }
            return Post(new Uri(requestUri, UriKind.RelativeOrAbsolute), content);
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = content;
            return SendAsync(request);
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return PostAsync((Uri)null, content);
            }
            return PostAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content);
        }

        public HttpResponseMessage Put(Uri requestUri, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            request.Content = content;
            return Send(request);
        }

        public HttpResponseMessage Put(string requestUri, HttpContent content)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return Put((Uri)null, content);
            }
            return Put(new Uri(requestUri, UriKind.RelativeOrAbsolute), content);
        }

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            request.Content = content;
            return SendAsync(request);
        }

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return PutAsync((Uri)null, content);
            }
            return PutAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content);
        }

        public HttpResponseMessage Delete(Uri requestUri)
        {
            return Send(new HttpRequestMessage(HttpMethod.Delete, requestUri));
        }

        public HttpResponseMessage Delete(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return Delete((Uri)null);
            }
            return Delete(new Uri(requestUri, UriKind.RelativeOrAbsolute));
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri));
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                return DeleteAsync((Uri)null);
            }
            return DeleteAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute));
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;

                // Cancel all pending requests (if any). Note that we don't call CancelPendingRequests() but cancel
                // the CTS directly. The reason is that CancelPendingRequests() would cancel the current CTS and create
                // a new CTS. We don't want a new CTS in this case.
                pendingRequestsCts.Cancel();

                // If we used a user-defined channel, dispose that channel. If no channel was specified, dispose the
                // default channel.
                if (userChannel != null)
                {
                    userChannel.Dispose();
                }
                else
                {
                    if (actualChannel != null)
                    {
                        actualChannel.Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void InitializeValues(Uri baseAddress)
        {
            CheckBaseAddress(baseAddress, "baseAddress");

            this.baseAddress = baseAddress;
            this.timeout = defaultTimeout;
            this.maxResponseContentBufferSize = HttpContent.DefaultMaxBufferSize;
            this.pendingRequestsCts = new CancellationTokenSource();
        }

        private void DisposeRequestContent(HttpRequestMessage request)
        {
            Contract.Requires(request != null);

            // When a request completes, HttpClient disposes the request content so the user doesn't have to. This also
            // ensures that a HttpContent object is only sent once using HttpClient (similar to HttpRequestMessages
            // that can also be sent only once).
            HttpContent content = request.Content;
            if (content != null)
            {
                content.Dispose();
            }
        }

        private void StartContentBuffering(HttpRequestMessage request, CancellationTokenSource cancellationTokenSource,
            TaskCompletionSource<HttpResponseMessage> tcs, HttpResponseMessage response)
        {
            response.Content.LoadIntoBufferAsync(maxResponseContentBufferSize).ContinueWith(contentTask =>
            {
                try
                {
                    // Make sure to dispose the CTS _before_ setting TaskCompletionSource. Otherwise the task will be
                    // completed and the user may dispose the user CTS on the continuation task leading to a race cond.
                    bool isCancellationRequested = cancellationTokenSource.Token.IsCancellationRequested;

                    // contentTask.Exception is always != null if IsFaulted is true. However, we need to access the
                    // Exception property, otherwise the Task considers the excpetion as "unhandled" and will throw in
                    // its finalizer.
                    if (contentTask.IsFaulted)
                    {
                        // If the cancellation token was canceled, we consider the exception to be caused by the
                        // cancellation (e.g. WebException when reading from canceled response stream).
                        if (isCancellationRequested && (contentTask.Exception.GetBaseException() is HttpException))
                        {
                            SetTaskCanceled(request, cancellationTokenSource, tcs);
                        }
                        else
                        {
                            SetTaskFaulted(request, cancellationTokenSource, tcs, contentTask.Exception.GetBaseException());
                        }
                        return;
                    }

                    if (contentTask.IsCanceled)
                    {
                        SetTaskCanceled(request, cancellationTokenSource, tcs);
                        return;
                    }

                    // When buffering content is completed, set the Task as completed.
                    SetTaskCompleted(request, cancellationTokenSource, tcs, response);
                }
                catch (Exception e)
                {
                    // Make sure we catch any exception, otherwise the task will catch it and throw in the finalizer.
                    tcs.TrySetException(e);
                }

            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private void SetOperationStarted()
        {
            // This method flags the HttpClient instances as "active". I.e. we executed at least one request (or are
            // in the process of doing so). This information is used to lock-down all property setters. Once a
            // Send/SendAsync operation started, no property can be changed.
            if (!operationStarted)
            {
                operationStarted = true;
            }
        }

        private void CheckDisposedOrStarted()
        {
            CheckDisposed();
            if (operationStarted)
            {
                throw new InvalidOperationException("The HttpClient instance already started one or more requests. Properties can only be modified before sending the first request.");
            }
        }

        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private static void CheckRequestMessage(HttpRequestMessage request)
        {
            if (!request.MarkAsSent())
            {
                throw new InvalidOperationException("The request message was already sent. Cannot send the same request message multiple times.");
            }
        }

        private void PrepareRequestMessage(HttpRequestMessage request)
        {
            Uri requestUri = null;
            if ((request.RequestUri == null) && (baseAddress == null))
            {
                throw new InvalidOperationException("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.");
            }
            if (request.RequestUri == null)
            {
                requestUri = baseAddress;
            }
            else
            {
                // If the request Uri is an absolute Uri, just use it. Otherwise try to combine it with the base Uri.
                if (!request.RequestUri.IsAbsoluteUri)
                {
                    if (baseAddress == null)
                    {
                        throw new InvalidOperationException("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.");
                    }
                    else
                    {
                        requestUri = new Uri(baseAddress, request.RequestUri);
                    }
                }
            }

            // We modified the original request Uri. Assign the new Uri to the request message.
            if (requestUri != null)
            {
                request.RequestUri = requestUri;
            }

            // Add default headers
            if (defaultRequestHeaders != null)
            {
                request.Headers.AddHeaders(defaultRequestHeaders);
            }
        }

        private static void CheckBaseAddress(Uri baseAddress, string parameterName)
        {
            if (baseAddress == null)
            {
                return; // It's OK to not have a base address specified.
            }

            if (!baseAddress.IsAbsoluteUri)
            {
                throw new ArgumentException("The base address must be an absolute URI.", parameterName);
            }

            if (!HttpUtilities.IsHttpUri(baseAddress))
            {
                throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", parameterName);
            }
        }

        private void SetTaskFaulted(HttpRequestMessage request, CancellationTokenSource cancellationTokenSource,
            TaskCompletionSource<HttpResponseMessage> tcs, Exception e)
        {
            LogSendError(request, cancellationTokenSource, "SendAsync", e);
            tcs.TrySetException(e);
        }

        private void SetTaskCanceled(HttpRequestMessage request, CancellationTokenSource cancellationTokenSource,
            TaskCompletionSource<HttpResponseMessage> tcs)
        {
            LogSendError(request, cancellationTokenSource, "SendAsync", null);
            tcs.TrySetCanceled();
        }

        private void SetTaskCompleted(HttpRequestMessage request, CancellationTokenSource cancellationTokenSource,
            TaskCompletionSource<HttpResponseMessage> tcs, HttpResponseMessage response)
        {
            tcs.TrySetResult(response);

            // TODO: 901737 Revert this commented code when move to 4.5
            // cancellationTokenSource.Dispose();
        }

        private void LogSendError(HttpRequestMessage request, CancellationTokenSource cancellationTokenSource,
            string method, Exception e)
        {
            Contract.Requires(request != null);

            // TODO not implemented.
        }

        private void SetTimeout(CancellationTokenSource cancellationTokenSource)
        {
            Contract.Requires(cancellationTokenSource != null);

            // TODO not implemented
        }
    }
}
