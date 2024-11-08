namespace Hospital.Application.Dtos.Auth
{
    public class ActionDto
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsInternal { get; set; }

        public int Exponent { get; set; }
    }
}
