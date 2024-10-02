using Hospital.SharedKernel.Domain.Constants;
using System.Runtime.Serialization;

namespace Hospital.SharedKernel.Runtime.Exceptions
{
    public class BadRequestException : Exception
    {
        public string Code { get; set; } = ErrorCodeConstant.BAD_REQUEST;

        public object Body { get; set; }

        public BadRequestException() : this(string.Empty)
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string code, string message) : this(message)
        {
            Code = code;
        }

        public BadRequestException(string code, string message, object body) : this(code, message)
        {
            Body = body;
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
