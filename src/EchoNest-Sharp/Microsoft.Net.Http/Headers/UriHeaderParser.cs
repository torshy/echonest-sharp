using System;

namespace System.Net.Http.Headers
{
    // Don't derive from BaseHeaderParser since parsing is delegated to Uri.TryCreate() 
    // which will remove leading and trailing whitespaces.
    internal class UriHeaderParser : HttpHeaderParser
    {
        private UriKind uriKind;

        internal static readonly UriHeaderParser RelativeOrAbsoluteUriParser = 
            new UriHeaderParser(UriKind.RelativeOrAbsolute);

        private UriHeaderParser(UriKind uriKind)
            : base(false)
        {
            this.uriKind = uriKind;
        }

        public override bool TryParseValue(string value, object storeValue, ref int index, out object parsedValue)
        {
            parsedValue = null;

            // Some headers support empty/null values. This one doesn't.
            if (string.IsNullOrEmpty(value) || (index == value.Length))
            {
                return false;
            }

            string uriString = value;
            if (index > 0)
            {
                uriString = value.Substring(index);
            }

            Uri uri;
            if (!Uri.TryCreate(uriString, uriKind, out uri))
            {
                return false;
            }

            index = value.Length;
            parsedValue = uri;
            return true;
        }
    }
}
