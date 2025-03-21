using System.ComponentModel;

namespace Hospital.Domain.Enums
{
    public enum Shift
    {
        None = 0,
        [Description("Ca sáng")]
        Morning = 1,

        [Description("Ca chiều")]
        Afternoon = 2,

        [Description("Ca tối")]
        Night = 3,
    }
}
