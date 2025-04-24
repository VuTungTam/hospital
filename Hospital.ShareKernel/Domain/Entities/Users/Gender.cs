using System.ComponentModel;

namespace Hospital.SharedKernel.Domain.Entities.Users
{
    public enum Gender
    {
        [Description("Nam")]
        Male = 1,

        [Description("Nữ")]
        Female = 0,
    }
}
