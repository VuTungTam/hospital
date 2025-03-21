using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;


namespace Hospital.Application.Commands.Customers
{
    [RequiredPermission(ActionExponent.Update)]
    public class UpdateCustomerProfileCommand : BaseCommand
    {
        public UpdateCustomerProfileCommand(UpdateProfileModel model)
        {
            Model = model;
        }

        public UpdateProfileModel Model { get; }
    }
}
