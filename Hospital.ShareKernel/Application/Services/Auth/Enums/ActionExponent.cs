using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel;

namespace Hospital.SharedKernel.Application.Services.Auth.Enums
{
    public enum ActionExponent : int
    {
        AllowAnonymous = -1,

        [Description("Quản trị hệ thống")]
        Master = 100,

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

        [Description("Quản lý hồ sơ")]
        ProfileManagement = 8,

        [Description("Thêm hồ sơ"), ParentObjectId(10)]
        AddProfile = 9,

        [Description("Cập nhật hồ sơ"), ParentObjectId(10)]
        UpdateProfile = 10,

        [Description("Xóa hồ sơ"), ParentObjectId(10)]
        DeleteProfile = 11,

        [Description("Xem danh sách hồ sơ"), ParentObjectId(10)]
        ViewProfile = 12,

        [Description("Quản lý đánh giá")]
        FeedbackManagement = 13,

        [Description("Thêm đánh giá"), ParentObjectId(15)]
        AddFeedback = 14,

        [Description("Cập nhật đánh giá"), ParentObjectId(15)]
        UpdateFeedback = 15,

        [Description("Xem danh sách đánh giá"), ParentObjectId(15)]
        ViewFeedback = 16,

        [Description("Quản lý dịch vụ")]
        ServiceManagement = 17,

        [Description("Thêm dịch vụ"), ParentObjectId(19)]
        AddService = 18,

        [Description("Cập nhật dịch vụ"), ParentObjectId(19)]
        UpdateService = 19,

        [Description("Xóa dịch vụ"), ParentObjectId(19)]
        DeleteService = 20,

        [Description("Xem danh sách dịch vụ"), ParentObjectId(19)]
        ViewService = 21,

        [Description("Quản lý lịch khám")]
        BookingManagement = 22,

        [Description("Thêm lịch khám"), ParentObjectId(24)]
        AddBooking = 23,

        [Description("Cập nhật lịch khám"), ParentObjectId(24)]
        UpdateBooking = 24,

        [Description("Xóa lịch khám"), ParentObjectId(24)]
        DeleteBooking = 25,

        [Description("Xem danh sách lịch khám"), ParentObjectId(24)]
        ViewBooking = 26,

        [Description("Quản lý khách hàng")]
        CustomerManagement = 27,

        [Description("Thêm khách hàng"), ParentObjectId(29)]
        AddCustomer = 28,

        [Description("Cập nhật khách hàng"), ParentObjectId(29)]
        UpdateCustomer = 29,

        [Description("Xóa khách hàng"), ParentObjectId(29)]
        DeleteCustomer = 30,

        [Description("Xem danh sách khách hàng"), ParentObjectId(29)]
        ViewCustomer = 31,

        [Description("Quản lý nhân viên")]
        EmployeeManagement = 32,

        [Description("Thêm nhân viên"), ParentObjectId(34)]
        AddEmployee = 33,

        [Description("Cập nhật nhân viên"), ParentObjectId(34)]
        UpdateEmployee = 34,

        [Description("Xóa nhân viên"), ParentObjectId(34)]
        DeleteEmployee = 35,

        [Description("Xem danh sách nhân viên"), ParentObjectId(34)]
        ViewEmployee = 36,

        [Description("Quản lý bác sĩ")]
        DoctorManagement = 37,

        [Description("Thêm bác sĩ"), ParentObjectId(39)]
        AddDoctor = 38,

        [Description("Cập nhật bác sĩ"), ParentObjectId(39)]
        UpdateDoctor = 39,

        [Description("Xóa bác sĩ"), ParentObjectId(39)]
        DeleteDoctor = 40,

        [Description("Xem danh sách bác sĩ"), ParentObjectId(39)]
        ViewDoctor = 41,

        [Description("Gửi yêu cầu")]
        SendRequest = 42,

        [Description("Quản lý cơ sở")]
        FacilityManagement = 43,

        [Description("Thêm cơ sở"), ParentObjectId(45)]
        AddFacility = 44,

        [Description("Cập nhật cơ sở"), ParentObjectId(45)]
        UpdateFacility = 45,

        [Description("Xóa cơ sở"), ParentObjectId(45)]
        DeleteFacility = 46,

        [Description("Xem danh sách cơ sở"), ParentObjectId(45)]
        ViewFacility = 47,

        [Description("Quản lý tin tức"), ParentObjectId(61)]
        ArticleManagement = 48,

        [Description("Quản lý hình ảnh"), ParentObjectId(61)]
        ImagesManagement = 49,

        [Description("Quản lý mạng xã hội"), ParentObjectId(61)]
        NetworksManagement = 50,

        [Description("Quản lý triệu chứng")]
        SymptomManagement = 51,

        [Description("Quản lý chuyên khoa")]
        SpecialtyManagement = 52,

        [Description("Xem lịch sử hoạt động")]
        ViewAudit = 53,

        [Description("Truy cập trang quản trị")]
        AccessAdminPage = 54,

        [Description("Phê duyệt yêu cầu")]
        ApproveRequest = 55,

        [Description("Xem biểu đồ thống kê")]
        ViewDashboard = 56,

        [Description("Hủy lịch khám")]
        CancelBooking = 57,

        [Description("Xác nhận lịch khám")]
        ConfirmBooking = 58,

        [Description("Quản lý hiển thị")]
        UIManagement = 59,

        [Description("Bắt đầu lịch khám")]
        StartBooking = 60,

        [Description("Hoàn thành lịch khám")]
        CompleteBooking = 61,

        [Description("Đặt lịch khám")]
        BookingAppointment = 62,

        [Description("Thay đổi quyền")]
        ChangeRole = 63,
    }
}
