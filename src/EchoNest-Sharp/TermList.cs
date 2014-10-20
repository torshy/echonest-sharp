using System.Collections;
using System.Collections.Generic;

namespace EchoNest
{
    public class TermList : IEnumerable<Term>
    {
        #region Fields

        private readonly IList<Term> _terms;

        #endregion Fields

        #region Constructors

        public TermList()
        {
            _terms = new List<Term>();
        }

        #endregion Constructors

        #region Methods

        public Term Add(string term)
        {
            var newTerm = new Term(term, this);
            _terms.Add(newTerm);
            return newTerm;
        }

        public IList<Term> AddRange(TermList terms)
        {
            foreach (var term in terms)
            {
                _terms.Add(term);
            }

            return _terms;
        }
        
        public Term Add(Term term)
        {
            _terms.Add(term);
            return term;
        }

        public IEnumerator<Term> GetEnumerator()
        {
            return _terms.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Methods
    }
}
