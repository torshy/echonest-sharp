using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public abstract class HttpContent : IDisposable
    {
        private HttpContentHeaders headers;
        private MemoryStream bufferedContent;
        private bool disposed;
        private Stream contentReadStream;
        private bool canCalculateLength;

        internal const int DefaultMaxBufferSize = 65536; // 64KB

        public HttpContentHeaders Headers
        {
            get
            {
                if (headers == null)
                {
                    headers = new HttpContentHeaders(GetComputedOrBufferLength);
                }
                return headers;
            }
        }

        public Stream ContentReadStream
        {
            get
            {
                CheckDisposed();

                if (contentReadStream == null)
                {
                    if (IsBuffered)
                    {
                        // We cast bufferedContent.Length to 'int': The framework doesn't support arrays > int.MaxValue,
                        // so the length will always be in the 'int' range.
                        contentReadStream = new MemoryStream(bufferedContent.GetBuffer(), 0,
                            (int)bufferedContent.Length, false, false);
                    }
                    else
                    {
                        contentReadStream = CreateContentReadStream();
                    }
                }
                return contentReadStream;
            }
        }

        private bool IsBuffered
        {
            get { return bufferedContent != null; }
        }

        protected HttpContent()
        {
            // We start with the assumption that we can calculate the content length.
            this.canCalculateLength = true;
        }

        public string ReadAsString()
        {
            CheckDisposed();

            LoadIntoBuffer();

            if (bufferedContent.Length == 0)
            {
                return string.Empty;
            }

            // We don't validate the Content-Encoding header: If the content was encoded, it's the caller's 
            // responsibility to make sure to only call ReadAsString() on already decoded content. E.g. if the 
            // Content-Encoding is 'gzip' the user should set WebRequestChannel.AutomaticDecompression to get a 
            // decoded response stream.

            // If we do have encoding information in the 'Content-Type' header, use that information to convert
            // the content to a string.
            Encoding encoding = HttpRuleParser.DefaultHttpEncoding;
            if ((Headers.ContentType != null) && (Headers.ContentType.CharSet != null))
            {
                try
                {
                    encoding = Encoding.GetEncoding(Headers.ContentType.CharSet);
                }
                catch (ArgumentException e)
                {
                    throw new InvalidOperationException(
                        "The character set provided in ContentType is invalid. Cannot read content as string using an invalid character set.", e);
                }
            }

            return encoding.GetString(bufferedContent.GetBuffer(), 0, (int)bufferedContent.Length);
        }

        public byte[] ReadAsByteArray()
        {
            CheckDisposed();
            LoadIntoBuffer();
            return bufferedContent.ToArray();
        }

        protected abstract Task SerializeToStreamAsync(Stream stream, TransportContext context);

        public Task CopyToAsync(Stream stream, TransportContext context)
        {
            CheckDisposed();
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            // The try..catch is used, since both FromAsync() and SerializeToStreamAsync() may throw: E.g. if a HWR
            // gets aborted after a request is complete, but before the response stream is read, trying to read from
            // the response stream will throw (ConnectStream.BeginRead() may throw WebException, ObjectDisposedException).
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            try
            {
                Task task = null;
                if (IsBuffered)
                {
                    task = Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, bufferedContent.GetBuffer(), 0,
                        (int)bufferedContent.Length, null);
                }
                else
                {
                    task = SerializeToStreamAsync(stream, context);
                    CheckTaskNotNull(task);
                }

                // If the copy operation fails, wrap the exception in an HttpException() if appropriate.
                task.ContinueWith(copyTask =>
                {
                    if (copyTask.IsFaulted)
                    {
                        tcs.TrySetException(GetStreamCopyException(copyTask.Exception.GetBaseException()));
                    }
                    else if (copyTask.IsCanceled)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        tcs.TrySetResult(null);
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);

            }
            catch (IOException e)
            {
                tcs.TrySetException(GetStreamCopyException(e));
            }
            catch (WebException e)
            {
                tcs.TrySetException(GetStreamCopyException(e));
            }
            catch (ObjectDisposedException e)
            {
                tcs.TrySetException(GetStreamCopyException(e));
            }

            return tcs.Task;
        }

        public Task CopyToAsync(Stream stream)
        {
            return CopyToAsync(stream, null);
        }

        protected abstract void SerializeToStream(Stream stream, TransportContext context);

        public void CopyTo(Stream stream, TransportContext context)
        {
            CheckDisposed();
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            try
            {
                if (IsBuffered)
                {
                    stream.Write(bufferedContent.GetBuffer(), 0, (int)bufferedContent.Length);
                }
                else
                {
                    SerializeToStream(stream, context);
                }
            }
            catch (IOException e)
            {
                throw GetStreamCopyException(e);
            }
            catch (WebException e)
            {
                throw GetStreamCopyException(e);
            }
            catch (ObjectDisposedException e)
            {
                throw GetStreamCopyException(e);
            }
        }

        public void CopyTo(Stream stream)
        {
            CopyTo(stream, null);
        }

        public Task LoadIntoBufferAsync()
        {
            return LoadIntoBufferAsync(DefaultMaxBufferSize);
        }

        // No "CancellationToken" parameter needed since canceling the CTS will close the connection, resulting
        // in an exception being thrown while we're buffering.
        // If buffering is used without a connection, it is supposed to be fast, thus no cancellation required.
        public Task LoadIntoBufferAsync(int maxBufferSize)
        {
            CheckDisposed();

            if (IsBuffered)
            {
                // If we already buffered the content, just return a completed task.
                return CreateCompletedTask();
            }

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            Exception error = null;
            MemoryStream tempBuffer = CreateMemoryStream(maxBufferSize, out error);

            if (tempBuffer == null)
            {
                // We don't throw in LoadIntoBufferAsync(): set the task as faulted and return the task.
                Contract.Assert(error != null);
                tcs.TrySetException(error);
            }
            else
            {
                // SerializeToStreamAsync() may throw, e.g. when trying to read from a ConnectStream where the HWR was
                // aborted. Make sure to catch these exceptions and let the task fail.
                try
                {
                    Task task = SerializeToStreamAsync(tempBuffer, null);
                    CheckTaskNotNull(task);

                    task.ContinueWith(copyTask =>
                    {
                        try
                        {
                            if (copyTask.IsFaulted)
                            {
                                tempBuffer.Dispose(); // Cleanup partially filled stream.
                                tcs.TrySetException(GetStreamCopyException(copyTask.Exception.GetBaseException()));
                                return;
                            }

                            if (copyTask.IsCanceled)
                            {
                                tempBuffer.Dispose(); // Cleanup partially filled stream.
                                tcs.TrySetCanceled();
                                return;
                            }

                            bufferedContent = tempBuffer;
                            tcs.TrySetResult(null);
                        }
                        catch (Exception e)
                        {
                            // Make sure we catch any exception, otherwise the task will catch it and throw in the finalizer.
                            tcs.TrySetException(e);
                        }

                        // Since we're not doing any CPU and/or I/O intensive operations, continue on the same thread.
                        // This results in better performance since the continuation task doesn't get scheduled by the
                        // scheduler and there are no context switches required.
                    }, TaskContinuationOptions.ExecuteSynchronously);

                }
                catch (IOException e)
                {
                    tcs.TrySetException(GetStreamCopyException(e));
                }
                catch (WebException e)
                {
                    tcs.TrySetException(GetStreamCopyException(e));
                }
                catch (ObjectDisposedException e)
                {
                    tcs.TrySetException(GetStreamCopyException(e));
                }
            }

            return tcs.Task;
        }

        public void LoadIntoBuffer()
        {
            LoadIntoBuffer(DefaultMaxBufferSize);
        }

        public void LoadIntoBuffer(int maxBufferSize)
        {
            CheckDisposed();

            if (!IsBuffered)
            {
                Exception error = null;
                MemoryStream tempBuffer = CreateMemoryStream(maxBufferSize, out error);

                if (tempBuffer == null)
                {
                    Contract.Assert(error != null);
                    throw error;
                }

                try
                {
                    SerializeToStream(tempBuffer, null);
                }
                catch (IOException e)
                {
                    tempBuffer.Dispose();
                    throw GetStreamCopyException(e);
                }
                catch (WebException e)
                {
                    tempBuffer.Dispose();
                    throw GetStreamCopyException(e);
                }
                catch (ObjectDisposedException e)
                {
                    tempBuffer.Dispose();
                    throw GetStreamCopyException(e);
                }
                catch (Exception)
                {
                    tempBuffer.Dispose();
                    throw;
                }

                // Only assign to bufferedContent if there was no error during buffering.
                bufferedContent = tempBuffer;
            }
        }

        protected virtual Stream CreateContentReadStream()
        {
            // By default just buffer the content to a memory stream. Derived classes can override this behavior
            // if there is a better way to retrieve the content as stream (e.g. byte array/string use a more efficient
            // way, like wrapping a read-only MemoryStream around the bytes/string)
            LoadIntoBuffer();

            return bufferedContent;
        }

        // Derived types return true if they're able to compute the length. It's OK if derived types return false to
        // indicate that they're not able to compute the length. The transport channel needs to decide what to do in
        // that case (send chunked, buffer first, etc.).
        protected internal abstract bool TryComputeLength(out long length);

        private long? GetComputedOrBufferLength()
        {
            CheckDisposed();

            if (IsBuffered)
            {
                return bufferedContent.Length;
            }

            // If we already tried to calculate the length, but the derived class returned 'false', then don't try
            // again; just return null.
            if (canCalculateLength)
            {
                long length = 0;
                if (TryComputeLength(out length))
                {
                    return length;
                }

                // Set flag to make sure next time we don't try to compute the length, since we know that we're unable
                // to do so.
                canCalculateLength = false;
            }
            return null;
        }

        private MemoryStream CreateMemoryStream(int maxBufferSize, out Exception error)
        {
            Contract.Ensures((Contract.Result<MemoryStream>() != null) ||
                (Contract.ValueAtReturn<Exception>(out error) != null));

            error = null;

            // If we have a Content-Length allocate the right amount of buffer up-front. Also check whether the
            // content length exceeds the max. buffer size.
            long? contentLength = Headers.ContentLength;

            if (contentLength != null)
            {
                Contract.Assert(contentLength >= 0);

                if (contentLength > maxBufferSize)
                {
                    error = new HttpException(string.Format(
                        "Cannot write more bytes to the buffer than the configured maximum buffer size: {0}.", maxBufferSize));
                    return null;
                }

                // We can safely cast contentLength to (int) since we just checked that it is <= maxBufferSize.
                return new LimitMemoryStream(maxBufferSize, (int)contentLength);
            }

            // We couldn't determine the length of the buffer. Create a memory stream with an empty buffer.
            return new LimitMemoryStream(maxBufferSize, 0);
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;

                if (contentReadStream != null)
                {
                    contentReadStream.Dispose();
                }

                if (IsBuffered)
                {
                    bufferedContent.Dispose();
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

        private void CheckTaskNotNull(Task task)
        {
            if (task == null)
            {
                throw new InvalidOperationException("The async operation did not return a System.Threading.Tasks.Task object.");
            }
        }

        private static Task CreateCompletedTask()
        {
            TaskCompletionSource<object> completed = new TaskCompletionSource<object>();
            bool resultSet = completed.TrySetResult(null);
            Contract.Assert(resultSet, "Can't set Task as completed.");
            return completed.Task;
        }

        private static Exception GetStreamCopyException(Exception originalException)
        {
            // HttpContent derived types should throw HttpExceptions if there is an error. However, since the stream
            // provided by CopyTo() can also throw, we wrap such exceptions in HttpException. This way custom content
            // types don't have to worry about it. The goal is that users of HttpContent don't have to catch multiple
            // exceptions (depending on the underlying transport), but just HttpExceptions (like HWR users just catch
            // WebException).
            // Note that Stream derived types should throw IOException, however our ConnectStream throws WebException.
            // So we wrap both. Custom stream should throw either IOException or HttpException.
            // We don't want to wrap other exceptions thrown by Stream (e.g. InvalidOperationException), since we
            // don't want to hide such "usage error" exceptions in HttpException.
            // ObjectDisposedException is also wrapped, since aborting HWR after a request is complete will result in
            // the response stream being closed.
            Exception result = originalException;
            if ((result is IOException) || (result is WebException) || (result is ObjectDisposedException))
            {
                result = new HttpException("Error while copying content to a stream.", result);
            }
            return result;
        }

        private class LimitMemoryStream : MemoryStream
        {
            private int maxSize;

            public LimitMemoryStream(int maxSize, int capacity)
                : base(capacity)
            {
                this.maxSize = maxSize;
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                CheckSize(count);
                return base.BeginWrite(buffer, offset, count, callback, state);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                CheckSize(count);
                base.Write(buffer, offset, count);
            }

            public override void WriteByte(byte value)
            {
                CheckSize(1);
                base.WriteByte(value);
            }

            private void CheckSize(int countToAdd)
            {
                if (maxSize - Length < countToAdd)
                {
                    throw new HttpException(string.Format("Cannot write more bytes to the buffer than the configured maximum buffer size: {0}.", maxSize));
                }
            }
        }
    }
}
