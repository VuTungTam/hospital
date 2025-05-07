using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Libraries.ExtensionMethods;

namespace Hospital.SharedKernel.Presentations.SignalR.Models
{
    public class SocketConnectionStatistics
    {
        public string ConnectionId { get; set; }

        public DateTime AccessDate { get; set; }

        public string Ip { get; set; }

        public string UserAgent { get; set; }

        public AccountType Type { get; set; }

        public string TypeText => Type.GetDescription();

        public string Uid { get; set; }

        public string Fullname { get; set; }

        public string Email { get; set; }

        public int PlaceCount { get; set; }

        public string PlaceCountText => "Trên " + PlaceCount + " trình duyệt";

        public List<SocketConnection> Tabs { get; set; } = new();

        public int OtherTabCount => Tabs?.Count ?? 0;

        public int TabCount => OtherTabCount + 1;
    }
}
