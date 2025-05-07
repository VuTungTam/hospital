using Hospital.SharedKernel.Configures.Models;

namespace Hospital.Application.Dtos.Images
{
    public class ImageDto : BaseDto
    {
        public string FacilityId { get; set; }

        public string PublicId { get; set; }

        public string ImageUrl => CdnConfig.Get(PublicId);
    }
}
