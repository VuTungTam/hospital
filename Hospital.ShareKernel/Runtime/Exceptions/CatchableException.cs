using System.Runtime.Serialization;

namespace Hospital.SharedKernel.Runtime.Exceptions
{
    public class CatchableException : Exception
    {
        public CatchableException() : this(string.Empty)
        {
        }

        public CatchableException(string message) : base(message)
        {
        }

        public CatchableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CatchableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
