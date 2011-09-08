using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Net.Mime;

namespace System.Net.Http
{
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "Represents a multipart/form-data content. Even if a collection of HttpContent is stored, " +
        "suffix Collection is not appropriate.")]
    public class MultipartFormDataContent : MultipartContent
    {
        private const string contentDisposition = "Content-Disposition";
        private const string formData = "form-data";

        public MultipartFormDataContent()
            : base(formData)
        {
        }

        public MultipartFormDataContent(string boundary)
            : base(formData, boundary)
        { 
        }

        public override void Add(HttpContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            Contract.EndContractBlock();

            if (!content.Headers.Contains(contentDisposition))
            {
                content.Headers.Add(contentDisposition, formData);
            }
            base.Add(content);
        }

        public void Add(HttpContent content, string name)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The value cannot be null or empty.", "name");
            }
            Contract.EndContractBlock();

            AddInternal(content, name, null);
        }

        public void Add(HttpContent content, string name, string fileName)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The value cannot be null or empty.", "name");
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("The value cannot be null or empty.", "fileName");
            }
            Contract.EndContractBlock();

            AddInternal(content, name, fileName);
        }

        private void AddInternal(HttpContent content, string name, string fileName)
        {
            if (!content.Headers.Contains(contentDisposition))
            {
                // Does 'name' or 'filename' require MIME encoding or quoting?
                name = EncodeAndQuote(name);

                if (fileName == null)
                {
                    content.Headers.Add(contentDisposition, formData + "; name=" + name);
                }
                else
                {
                    fileName = EncodeAndQuote(fileName);

                    content.Headers.Add(contentDisposition, formData + "; name=" + name + "; filename=" + fileName);
                }
            }
            base.Add(content);
        }

        private string EncodeAndQuote(string name)
        {
            string result = name;
            // Remove bounding quotes, they'll get re-added later
            if (result.StartsWith("\"", StringComparison.Ordinal) && result.EndsWith("\"", StringComparison.Ordinal)
                && result.Length > 1)
            {
                result = result.Substring(1, result.Length - 2);
            }
            if (result.Contains("\"")) // Only bounding quotes are allowed
            {
                throw new ArgumentException(string.Format("The format of value '{0}' is invalid.", name));
            }
            if (RequiresEncoding(result))
            {
                result = Encode(result); // =?utf-8?B?asdfasdfaesdf?=
            }
            // Re-add quotes
            return "\"" + result + "\""; // "value", required by IIS
        }

        private bool RequiresEncoding(string name)
        {
            // TODO not implemented.
            return false;
        }

        private string Encode(string name)
        {
            // TODO not implemented.
            return name;
        }
    }
}
