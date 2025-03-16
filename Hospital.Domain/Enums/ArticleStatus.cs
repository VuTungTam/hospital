using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum ArticleStatus
    {
        None = 0,

        [Description("Đang hoạt động")]
        Active = 1,

        [Description("Bản nháp")]
        Draft = 2,

        [Description("Tạm ẩn")]
        Hidden = 3,
    }
}
