using System.ComponentModel;

namespace Hospital.SharedKernel.Application.Services.Auth.Enums
{
    public enum ActionExponent : int
    {
        AllowAnonymous = -1,

        [Description("Tất cả các quyền")]
        Master = 32,

        [Description("Quản lý chi nhánh")]
        BranchManagement = 31,

        [Description("Xem")]
        View = 0,

        [Description("Thêm")]
        Add = 1,

        [Description("Sửa")]
        Update = 2,

        [Description("Xóa")]
        Delete = 3,

        [Description("Xuất file")]
        Export = 4,

        [Description("Import file")]
        Import = 5,

        [Description("Upload")]
        Upload = 6,

        [Description("Download")]
        Download = 7,

        [Description("Quản lý khách hàng")]
        CustomerManagement = 8,

        [Description("Quản lý nhân viên")]
        EmployeeManagement = 9,

        [Description("Quản lý dịch vụ")]
        ServiceManagement = 10,

        [Description("Quản lý hiển thị")]
        UIManagement = 11,

        [Description("Quản lý triệu chứng")]
        SymptomManagement = 12,

        [Description("Quản lý tuyển dụng")]
        RecruitmentManagement = 13,

        [Description("Quản lý lịch hẹn")]
        BookingManagement = 14,

        [Description("Quản lý lịch trực")]
        CanlendarManagement = 15,

        [Description("Xem lịch sử hoạt động")]
        ViewAudit = 22,

        [Description("Truy cập trang quản trị")]
        AccessAdminPage = 23,
    }
}
