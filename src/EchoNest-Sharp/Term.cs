using System;
using System.Globalization;
using System.Web;

namespace EchoNest
{
    public class Term
    {
        #region Fields

        private TermList _parent;
        private bool? _ban;
        private double? _boost;
        private bool? _require;
        private string _term;

        #endregion Fields

        #region Constructors

        public Term()
        {
        }

        public Term(string term, TermList parent)
        {
            _term = term;
            _parent = parent;
        }

        #endregion Constructors

        #region Properties

        public string Name
        {
            get { return _term; }
            set { _term = value; }
        }

        public bool? IsBanned
        {
            get { return _ban; }
            set
            {
                if (_require.HasValue || _boost.HasValue)
                {
                    throw new InvalidOperationException("Cannot have a inclusion or exclusion parameter and a boost.");
                }

                _ban = value;
            }
        }

        public double? IsBoosted
        {
            get { return _boost; }
            set
            {
                if (_ban.HasValue || _require.HasValue)
                {
                    throw new InvalidOperationException("Cannot have a inclusion or exclusion parameter and a boost.");
                }

                _boost = value;
            }
        }

        public bool? IsRequired
        {
            get { return _require; }
            set
            {
                if (_ban.HasValue || _boost.HasValue)
                {
                    throw new InvalidOperationException("Cannot have a inclusion or exclusion parameter and a boost.");
                }

                _require = value;
            }
        }

        #endregion Properties

        #region Methods

        public Term Add(string term)
        {
            if (_parent != null)
            {
                return _parent.Add(term);
            }

            throw new InvalidOperationException("Not attached to any parent. Unable to add term");
        }

        public Term Ban()
        {
            IsBanned = true;
            return this;
        }

        public Term Boost(double boost)
        {
            IsBoosted = boost;
            return this;
        }

        public Term Require()
        {
            IsRequired = true;
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