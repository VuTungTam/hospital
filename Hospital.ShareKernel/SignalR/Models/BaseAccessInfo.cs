using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;

namespace Hospital.SharedKernel.SignalR.Models
{
    public class BaseAccessInfo
    {
        public string ConnectionId { get; set; }

        public AccountType Type { get; set; }

        public DateTime AccessDate { get; set; }

        public string Ip { get; set; }

        public string UserAgent { get; set; }

        public string TypeText => Type.GetDescription();
    }
}
