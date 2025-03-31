using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum DoctorTitle
    {
        None = 0,

        [Description("Thạc sĩ")]
        Ths = 1,

        [Description("Tiến sĩ")]
        TS = 2,

        [Description("Tiến sĩ khoa học")]
        TSKH = 3
    }
}
