using FluentValidation;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Users;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Employee
{
    public class EmployeeDto : BaseUserDto
    {
        public string ScheduleColor { get; set; }

        public List<RoleDto> Roles { get; set; }

        public List<ActionDto> Actions { get; set; }

        public string RoleNames => string.Join(", ", Roles?.Select(x => x.Name) ?? new List<string>());

    }

    public class EmployeeDtoValidator : BaseUserDtoDtoValidator<EmployeeDto>
    {
        public EmployeeDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x).Must(x => x.Roles != null && x.Roles.Any()).WithMessage("Chưa chọn vai trò");
        }
    }
}
