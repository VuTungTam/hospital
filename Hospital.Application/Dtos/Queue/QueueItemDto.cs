namespace Hospital.Application.Dtos.Queue
{
    public class QueueItemDto : BaseDto
    {
        public string VisitId { get; set; }
        public int Position { get; set; }
        public int State { get; set; }
    }
}
