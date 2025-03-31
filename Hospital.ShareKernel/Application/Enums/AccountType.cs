using System.ComponentModel;

namespace Hospital.SharedKernel.Application.Enums
{
    public enum AccountType
    {
        [Description("Khách hàng")]
        Customer = 1,

        [Description("Nhân viên hệ thống")]
        Employee = 2,

        [Description("Bác sĩ")]
        Doctor = 3,

        [Description("Khách vãng lai")]
        Anonymous = 10,
    }
}
