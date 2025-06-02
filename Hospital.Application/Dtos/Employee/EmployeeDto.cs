using FluentValidation;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Dtos.Users;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Employee
{
    public class EmployeeDto : BaseUserDto
    {
        public List<RoleDto> Roles { get; set; }

        public List<ActionDto> Actions { get; set; }

        public string ZoneId { get; set; }

        public string FacilityId { get; set; }

        public string FacilityNameVn { get; set; }

        public string FacilityNameEn { get; set; }

        public string RoleNames => string.Join(", ", Roles?.Select(x => x.Name) ?? new List<string>());

        public string RoleNameEns => string.Join(", ", Roles?.Select(x => x.NameEn) ?? new List<string>());

    }

    public class EmployeeDtoValidator : BaseUserDtoDtoValidator<EmployeeDto>
    {
        public EmployeeDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x).Must(x => x.Roles != null && x.Roles.Any()).WithMessage(localizer["Roles.CannotEmpty"]);
            RuleFor(x => x.ZoneId).Must(x => long.TryParse(x, out var id) && id >= 0).WithMessage(localizer["CommonMessage.ZoneIsNotValid"]);
        }
    }
}
