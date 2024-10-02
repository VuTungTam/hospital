namespace Hospital.Application.Dtos
{
    public class BaseDto
    {
        public string Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
