namespace Hospital.Application.Dtos.Visits
{
    public class VisitDto : BaseDto
    {
        public long DeclarationId { get; set; }
        public long ServiceId { get; set; }
    }
}
