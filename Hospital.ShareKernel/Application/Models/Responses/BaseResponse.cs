namespace Hospital.SharedKernel.Application.Models.Responses
{
    public class BaseResponse
    {
        public string Code { get; set; } = "success";

        public string Message { get; set; }

        public BaseResponse()
        {
        }

        public BaseResponse(string message)
        {
            Message = message;
        }

        public BaseResponse(string code, string message) : this(message)
        {
            Code = code;
        }
    }
}
