using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;

namespace System.Net.Http
{
    internal static class HttpUtilities
    {
        internal static readonly Version DefaultVersion = HttpVersion.Version11;

        internal static bool IsHttpUri(Uri uri)
        {
            Contract.Assert(uri != null);

            string scheme = uri.Scheme;
            return ((string.Compare("http", scheme, StringComparison.OrdinalIgnoreCase) == 0) ||
                (string.Compare("https", scheme, StringComparison.OrdinalIgnoreCase) == 0));
        }
    }
}
