namespace Hospital.SharedKernel.Domain.Constants
{
    public class ErrorCode
    {
        public const string _401 = "0x20040001";
        public const string BAD_REQUEST = "0x80040000";
        public const string SQL_INJECTOR_DETECTED = "0x80040001";
        public const string PWD_NOT_VALID = "0x80040002";
        public const string CODE_EXISTED = "0x80040003";
        public const string UNKNOWN_ERROR = "0x80004005";
        public const string SERVER_ERROR = "0x80131500";
        public const string TOKEN_INVALID = "0x80431500";
    }
}
