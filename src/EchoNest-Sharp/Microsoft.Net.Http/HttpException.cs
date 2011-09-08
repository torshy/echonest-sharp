using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Net.Http
{
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors",
        Justification = "We use the .NET 4.0 safe-serialization (see use of SerializeObjectState).")]
    public class HttpException : Exception
    {
        private static readonly EventHandler<SafeSerializationEventArgs> handleSerialization = HandleSerialization;

        public HttpException()
            : this(null, null)
        { }

        public HttpException(string message)
            : this(message, null)
        { }

        public HttpException(string message, Exception inner)
            : base(message, inner)
        {
            SerializeObjectState += handleSerialization;
        }

        private static void HandleSerialization(object exception, SafeSerializationEventArgs eventArgs)
        {
            // Nothing to do here, since we don't have any additional state. But setting the event is required in order 
            // to enable serialization. Note that we use the .NET 4 safe-serialization rather than the old 
            // ISerializable.
            // Also note that we use a static delegate since we don't have any instance state.
        }
    }

}
