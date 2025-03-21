using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.Update)]
    public class UpdateEmployeeProfileCommand : BaseCommand
    {
        public UpdateEmployeeProfileCommand(UpdateProfileModel model)
        {
            Model = model;
        }

        public UpdateProfileModel Model { get; }
    }
}
