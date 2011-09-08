using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace System.Net.Http
{
    // Send/SendAsync() are internal & protected methods. This way customers implementing a channel that delegates 
    // creation of response messages need to derive from this type. This provides a common experience for all 
    // customers. Omitting this type and changing Send/SendAsync() to public will cause customers to implement their
    // own "delegating channels" and also causes confusion because customers can use a channel directly to send 
    // requests (where they should use HttpClient instead).
    public abstract class DelegatingChannel : HttpMessageChannel
    {
        private HttpMessageChannel innerChannel;

        protected DelegatingChannel(HttpMessageChannel innerChannel)
        {
            if (innerChannel == null)
            {
                throw new ArgumentNullException("innerChannel");
            }

            this.innerChannel = innerChannel;
        }

        protected internal override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "A request message must be provided. It cannot be null.");
            }
            return innerChannel.Send(request, cancellationToken);
        }

        protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "A request message must be provided. It cannot be null.");
            }
            return innerChannel.SendAsync(request, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                innerChannel.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
