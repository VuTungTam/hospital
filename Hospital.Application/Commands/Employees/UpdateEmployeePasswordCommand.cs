using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Employees
{
    //[RequiredPermission(ActionExponent.UpdateEmployee)]
    public class UpdateEmployeePasswordCommand : BaseCommand
    {
        public UpdateEmployeePasswordCommand(UserPwdModel model)
        {
            Model = model;
        }

        public UserPwdModel Model { get; }
    }
}
