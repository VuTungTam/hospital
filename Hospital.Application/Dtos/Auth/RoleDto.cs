namespace Hospital.Application.Dtos.Auth
{
    public class RoleDto
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NameEn { get; set; }

        public List<ActionDto> Actions { get; set; }
    }
}
