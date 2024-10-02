using System.ComponentModel;

namespace Hospital.SharedKernel.Domain.Enums
{
    public enum AccountStatus
    {
        None = 0,

        [Description("Đang hoạt động")]
        Active = 1,

        [Description("Ngừng hoạt động")]
        Inactive = 2,

        [Description("Bị khóa")]
        Blocked = 3,

        [Description("Chưa xác nhận")]
        UnConfirm = 4,
    }
}
