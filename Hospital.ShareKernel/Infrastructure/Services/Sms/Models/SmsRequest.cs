namespace Hospital.SharedKernel.Infrastructure.Services.Sms.Models
{
    public class SmsRequest
    {
        public string Phone { get; set; }

        public string Message { get; set; }

        public SmsRequest()
        {

        }

        public SmsRequest(string phone, string message)
        {
            Phone = phone;
            Message = message;
        }
    }
}
