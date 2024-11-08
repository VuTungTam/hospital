using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum BookingStatus
    {
        None = 0,

        [Description("Chờ xác nhận")]
        Waiting = 1,

        [Description("Đã xác nhận")]
        Confirmed = 2,

        [Description("Từ chối")]
        Rejected = 3,

        [Description("Đã hủy")]
        Cancel = 4,

        [Description("Đang khám")]
        Doing = 5,

        [Description("Đã khám")]
        Completed = 6
    }
}
