namespace Hospital.SharedKernel.Application.Services.Auth.Models
{
    public class BasicRequestInfo
    {
        public string Ip { get; set; }

        public string Method { get; set; }

        public string ApiUrl { get; set; }

        public string UA { get; set; }

        public string Origin { get; set; }
    }

    public class RequestInfo : BasicRequestInfo
    {
        private string _browser;

        private string _os;

        public string RequestId { get; set; }

        public string Browser
        {
            get
            {
                if (_browser == "Other")
                {
                    return "Unknown";
                }
                return _browser;
            }
            set
            {
                _browser = value;
            }
        }

        public string OS
        {
            get
            {
                if (_os == "Other")
                {
                    return "Unknown";
                }
                return _os;
            }
            set
            {
                _os = value;
            }
        }

        public string Device { get; set; }
    }
}
