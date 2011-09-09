using System;
using System.Globalization;
using System.Web;

namespace EchoNest
{
    public class Term
    {
        #region Fields

        private readonly TermList _parent;
        private readonly string _term;

        private bool? _ban;
        private double? _boost;
        private bool? _require;

        #endregion Fields

        #region Constructors

        public Term(string term, TermList parent)
        {
            _term = term;
            _parent = parent;
        }

        #endregion Constructors

        #region Methods

        public Term Add(string term)
        {
            return _parent.Add(term);
        }

        public Term Ban()
        {
            if (_require.HasValue || _boost.HasValue)
            {
                throw new InvalidOperationException("Cannot have a inclusion or exclusion parameter and a boost.");
            }

            _ban = true;
            return this;
        }

        public Term Boost(double boost)
        {
            if (_ban.HasValue || _require.HasValue)
            {
                throw new InvalidOperationException("Cannot have a inclusion or exclusion parameter and a boost.");
            }

            _boost = boost;
            return this;
        }

        public Term Require()
        {
            if (_ban.HasValue || _boost.HasValue)
            {
                throw new InvalidOperationException("Cannot have a inclusion or exclusion parameter and a boost.");
            }

            _require = true;
            return this;
        }

        public override string ToString()
        {
            string term = _term;

            if (_ban.HasValue)
            {
                term = "-" + _term;
            }

            if (_require.HasValue)
            {
                term = "+" + _term;
            }

            if (_boost.HasValue)
            {
                term = term + "^" + _boost.Value.ToString(CultureInfo.InvariantCulture);
            }

            return HttpUtility.HtmlEncode(term);
        }

        #endregion Methods
    }
}