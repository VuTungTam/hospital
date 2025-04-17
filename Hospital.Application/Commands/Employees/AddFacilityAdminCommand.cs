using Hospital.Application.Dtos.Employee;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Employees
{
    [RequiredPermission(ActionExponent.AddAdmin)]
    public class AddFacilityAdminCommand : BaseCommand<string>
    {
        public AddFacilityAdminCommand(AdminDto admin) 
        {
            Admin = admin;
        }
        public AdminDto Admin { get;}
    }
}
