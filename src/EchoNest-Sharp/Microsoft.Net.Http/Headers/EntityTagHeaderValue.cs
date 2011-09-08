using System.Diagnostics.Contracts;

namespace System.Net.Http.Headers
{
    public class EntityTagHeaderValue : ICloneable
    {
        private static EntityTagHeaderValue any;

        private string tag;
        private bool isWeak;

        public string Tag
        {
            get { return tag; }
        }

        public bool IsWeak
        {
            get { return isWeak; }
        }

        public static EntityTagHeaderValue Any
        {
            get
            {
                if (any == null)
                {
                    any = new EntityTagHeaderValue();
                    any.tag = "*";
                    any.isWeak = false;
                }
                return any;
            }
        }

        public EntityTagHeaderValue(string tag)
            : this(tag, false)
        {
        }

        public EntityTagHeaderValue(string tag, bool isWeak)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("The value cannot be null or empty.", "tag");
            }
            int length = 0;
            if ((HttpRuleParser.GetQuotedStringLength(tag, 0, out length) != HttpParseResult.Parsed) ||
                (length != tag.Length))
            {
                // Note that we don't allow 'W/' prefixes for weak ETags in the 'tag' parameter. If the user wants to
                // add a weak ETag, he can set 'isWeak' to true.
                throw new FormatException("The specified value is not a valid quoted string.");
            }

            this.tag = tag;
            this.isWeak = isWeak;
        }

        private EntityTagHeaderValue(EntityTagHeaderValue source)
        {
            Contract.Requires(source != null);

            this.tag = source.tag;
            this.isWeak = source.isWeak;
        }

        private EntityTagHeaderValue()
        {
        }

        public override string ToString()
        {
            if (isWeak)
            {
                return "W/" + tag;
            }
            return tag;
        }

        public override bool Equals(object obj)
        {
            EntityTagHeaderValue other = obj as EntityTagHeaderValue;

            if (other == null)
            {
                return false;
            }

            // Since the tag is a quoted-string we treat it case-sensitive.
            return ((isWeak == other.isWeak) && (string.CompareOrdinal(tag, other.tag) == 0));
        }

        public override int GetHashCode()
        {
            // Since the tag is a quoted-string we treat it case-sensitive.
            return tag.GetHashCode() ^ isWeak.GetHashCode();
        }

        internal static int GetEntityTagLength(string input, int startIndex, out EntityTagHeaderValue parsedValue)
        {
            Contract.Requires(startIndex >= 0);

            parsedValue = null;

            if (string.IsNullOrEmpty(input) || (startIndex >= input.Length))
            {
                return 0;
            }

            // Caller must remove leading whitespaces. If not, we'll return 0.
            bool isWeak = false;
            int current = startIndex;

            char firstChar = input[startIndex];
            if (firstChar == '*')
            {
                // We have '*' value, indicating "any" ETag.
                parsedValue = Any;
                current++;
            }
            else
            {
                // The RFC defines 'W/' as prefix, but we'll be flexible and also accept lower-case 'w'.
                if ((firstChar == 'W') || (firstChar == 'w'))
                {
                    current++;
                    // We need at least 3 more chars: the '/' character followed by two quotes.
                    if ((current + 2 >= input.Length) || (input[current] != '/'))
                    {
                        return 0;
                    }
                    isWeak = true;
                    current++; // we have a weak-entity tag.
                    current = current + HttpRuleParser.GetWhitespaceLength(input, current);
                }

                int tagStartIndex = current;
                int tagLength = 0;
                if (HttpRuleParser.GetQuotedStringLength(input, current, out tagLength) != HttpParseResult.Parsed)
                {
                    return 0;
                }

                parsedValue = new EntityTagHeaderValue();
                if (tagLength == input.Length)
                {
                    // Most of the time we'll have strong ETags without leading/trailing whitespaces.
                    Contract.Assert(startIndex == 0);
                    Contract.Assert(!isWeak);
                    parsedValue.tag = input;
                    parsedValue.isWeak = false;
                }
                else
                {
                    parsedValue.tag = input.Substring(tagStartIndex, tagLength);
                    parsedValue.isWeak = isWeak;
                }

                current = current + tagLength;
            }
            current = current + HttpRuleParser.GetWhitespaceLength(input, current);

            return current - startIndex;
        }

        object ICloneable.Clone()
        {
            if (this == any)
            {
                return any;
            }
            else
            {
                return new EntityTagHeaderValue(this);
            }
        }
    }
}
