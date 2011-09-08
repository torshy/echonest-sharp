using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public abstract class HttpMessageChannel : IDisposable
    {
        protected HttpMessageChannel()
        {
        }

        protected internal abstract HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken);
        protected internal abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        { 
            // Nothing to do in base class.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
