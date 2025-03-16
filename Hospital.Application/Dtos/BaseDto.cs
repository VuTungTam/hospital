namespace Hospital.Application.Dtos
{
    public class BaseDto
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
