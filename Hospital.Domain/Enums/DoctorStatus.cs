using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum DoctorStatus
    {
        None = 0,

        [Description("Chờ xác nhận")]
        Waiting = 1,

        [Description("Đang hoạt động")]
        InActive = 2,

        [Description("Đang hoạt động")]
        NotInActive = 3
    }
}
