namespace EchoNest
{
    public class IdSpace
    {
        #region Fields

        private readonly string _id;

        #endregion Fields

        #region Constructors

        public IdSpace(string id)
        {
            _id = id;
        }

        #endregion Constructors

        #region Properties

        public string Id
        {
            get { return _id; }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return _id;
        }

        #endregion Methods
    }
}