namespace Hospital.Application.Dtos.Customers
{
    public class CustomerNameDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Phone { get; set; }

        public string NameWithCode => $"{Name} ({Code})";
    }
}
