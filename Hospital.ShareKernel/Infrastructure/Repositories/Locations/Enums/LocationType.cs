using System.ComponentModel;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Locations.Enums
{
    public enum LocationType
    {
        [Description("Trung ương")]
        Municipality = 1,

        [Description("Tỉnh")]
        Province = 2,

        [Description("Thành phố thuộc tỉnh")]
        ProvincialCity = 3,

        [Description("Quận")]
        UrbanDistrict = 4,

        [Description("Thị xã")]
        DistrictLevelTown = 5,

        [Description("Huyện")]
        District = 6,

        [Description("Phường")]
        Ward = 7,

        [Description("Thị trấn")]
        CommuneLevelTown = 8,

        [Description("Xã")]
        Commune = 9,
    }
}
