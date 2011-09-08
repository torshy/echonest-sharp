using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public class StreamContent : HttpContent
    {
        private const int defaultBufferSize = 4096;

        private Stream content;
        private int bufferSize;
        private bool contentConsumed;

        public StreamContent(Stream content)
            : this(content, defaultBufferSize)
        {
        }

        public StreamContent(Stream content, int bufferSize)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            this.content = content;
            this.bufferSize = bufferSize;
        }

        protected override void SerializeToStream(Stream stream, TransportContext context)
        {
            Contract.Assert(stream != null);

            PrepareContent();
            try
            {
                content.CopyTo(stream, bufferSize);
            }
            finally
            {
                // Make sure the source stream gets disposed if it can't be consumed multiple times.
                if (!content.CanSeek)
                {
                    content.Dispose();
                }
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Contract.Assert(stream != null);

            PrepareContent();
            // If the stream can't be re-read, make sure that it gets disposed once it is consumed.
            StreamToStreamCopy sc = new StreamToStreamCopy(content, stream, bufferSize, !content.CanSeek);
            return sc.StartAsync();
        }

        protected internal override bool TryComputeLength(out long length)
        {
            if (content.CanSeek)
            {
                length = content.Length;
                return true;
            }
            else
            {
                length = 0;
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override Stream CreateContentReadStream()
        {
            // Wrap the stream with a read-only stream to prevent someone from writing to the stream. Note that the
            // caller can still write to the stream since he has a reference to it. However, if the content gets 
            // passed to other components (e.g. channel), they should not be able to write to the stream.
            return new ReadOnlyStream(content);
        }

        private void PrepareContent()
        {
            if (contentConsumed)
            {
                // If the content needs to be written to a target stream a 2nd time, then the stream must support
                // seeking (e.g. a FileStream), otherwise the stream can't be copied a second time to a target 
                // stream (e.g. a NetworkStream).
                if (content.CanSeek)
                {
                    content.Position = 0;
                }
                else
                {
                    throw new InvalidOperationException("The stream was already consumed. It cannot be read again.");
                }
            }

            contentConsumed = true;
        }

        private class ReadOnlyStream : Stream
        {
            private Stream innerStream;

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return innerStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override long Length
            {
                get { return innerStream.Length; }
            }

            public override long Position
            {
                get { return innerStream.Position; }
                set { innerStream.Position = value; }
            }

            public override int ReadTimeout
            {
                get { return innerStream.ReadTimeout; }
                set { innerStream.ReadTimeout = value; }
            }

            public override bool CanTimeout
            {
                get { return innerStream.CanTimeout; }
            }

            public override int WriteTimeout
            {
                get { throw new NotSupportedException("The stream does not support writing."); }
                set { throw new NotSupportedException("The stream does not support writing."); }
            }

            public ReadOnlyStream(Stream innerStream)
            {
                Contract.Assert(innerStream != null);

                this.innerStream = innerStream;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return innerStream.Seek(offset, origin);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return innerStream.Read(buffer, offset, count);
            }

            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                return innerStream.BeginRead(buffer, offset, count, callback, state);
            }

            public override int EndRead(IAsyncResult asyncResult)
            {
                return innerStream.EndRead(asyncResult);
            }

            public override int ReadByte()
            {
                return innerStream.ReadByte();
            }

            public override void Flush()
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            public override void WriteByte(byte value)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    innerStream.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
