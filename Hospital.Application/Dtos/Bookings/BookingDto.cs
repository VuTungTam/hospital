using Hospital.Application.Dtos.Symptoms;
using Hospital.Domain.Enums;

namespace Hospital.Application.Dtos.Bookings
{
    public class BookingDto : BaseDto
    {
        public string Code { get; set; }

        public string HealthProfileId { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime Date { get; set; }

        public string ServiceId { get; set; }

        public string ServiceNameVn;

        public string ServiceNameEn;

        public TimeSpan ServiceStartTime { get; set; }

        public TimeSpan ServiceEndTime { get; set; }

        public int Order { get; set; }

        public List<SymptomDto> Symptoms { get; set; }


        public List<string> SymptomNameVns;


        public List<string> SymptomNameEns;
    }
}