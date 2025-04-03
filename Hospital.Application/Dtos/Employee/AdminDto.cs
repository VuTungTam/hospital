using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Users;

namespace Hospital.Application.Dtos.Employee
{
    public class AdminDto : BaseUserDto
    {
        public List<RoleDto> Roles { get; set; }

        public string RoleNames => string.Join(", ", Roles?.Select(x => x.Name) ?? new List<string>());

    }
}
