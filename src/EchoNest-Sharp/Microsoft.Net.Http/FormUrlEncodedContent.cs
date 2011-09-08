using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics.Contracts;

namespace System.Net.Http
{
    public class FormUrlEncodedContent : HttpContent
    {
        // Note that using a Dictionary<string, string> doesn't allow duplicate key values. However, 
        // users can use List<KeyValuePair<string, string>> to provide lists with multiple names.

        private byte[] content;

        public FormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException("nameValueCollection");
            }
            Contract.EndContractBlock();

            Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Encode and concatenate data
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in nameValueCollection)
            {
                if (builder.Length > 0) 
                {
                    // Not first, add a seperator
                    builder.Append('&');
                }

                builder.Append(Encode(pair.Key));
                builder.Append('=');
                builder.Append(Encode(pair.Value));
            }

            content = HttpRuleParser.DefaultHttpEncoding.GetBytes(builder.ToString());
        }

        private string Encode(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return String.Empty;
            }
            // Escape spaces as '+'.
            return Uri.EscapeDataString(data).Replace("%20", "+"); 
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Contract.Assert(stream != null);

            return Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, content, 0, content.Length, null);
        }

        protected override void SerializeToStream(Stream stream, TransportContext context)
        {
            Contract.Assert(stream != null);

            stream.Write(content, 0, content.Length);
        }

        protected internal override bool TryComputeLength(out long length)
        {
            length = content.Length;
            return true;
        }

        protected override Stream CreateContentReadStream()
        {
            return new MemoryStream(content, 0, content.Length, false, false);
        }
    }
}
