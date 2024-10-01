using System.Runtime.Serialization;

namespace Hospital.SharedKernel.Runtime.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public object DataException { get; }

        public UnauthorizeException() : this(string.Empty)
        {
        }

        public UnauthorizeException(object dataException) : this(string.Empty)
        {
            DataException = dataException;
        }

        public UnauthorizeException(string message) : base(message)
        {
        }

        public UnauthorizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
