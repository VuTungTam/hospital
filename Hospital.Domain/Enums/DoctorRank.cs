using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum DoctorRank
    {
        None = 0,

        [Description("Bác sĩ")]
        BS = 1,

        [Description("Bác sĩ chuyên khoa I")]
        BSCKI = 2,

        [Description("Bác sĩ nội trú")]
        BSNT = 3,

        [Description("Bác sĩ chuyên khoa II")]
        BSCKII = 4,
    }
}
