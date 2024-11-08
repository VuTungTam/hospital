using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum HealthServiceStatus
    {
        None = 0,

        [Description("Đang hoạt động")]
        InActive = 1,

        [Description("Không hoạt động")]
        NotInActive = 2
    }
}
