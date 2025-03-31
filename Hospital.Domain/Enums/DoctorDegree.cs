using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum DoctorDegree
    {
        None = 0,

        [Description("Phó giáo sư")]
        PSG = 1,

        [Description("Giáo sư")]
        GS = 2
    }
}
