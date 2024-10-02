using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum BranchStatus
    {
        None = 0,

        [Description("Đang hoạt động")]
        Active = 1,

        [Description("Ngừng hoạt động")]
        Inactive = 2,
    }
}
