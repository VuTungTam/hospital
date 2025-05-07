using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;

namespace Hospital.SharedKernel.Presentations.SignalR.Models
{
    public class SocketConnection
    {
        public string ConnectionId { get; set; }

        public string Uid { get; set; }

        public AccountType Type { get; set; }

        public string TypeText => Type.GetDescription();

        public DateTime AccessDate { get; set; }

        public string Ip { get; set; }

        public string UserAgent { get; set; }
    }
}
