using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Customers
{
    [RequiredPermission(ActionExponent.UpdateCustomer)]
    public class UpdateCustomerStatusCommand : BaseCommand
    {
        public UpdateCustomerStatusCommand(long id, AccountStatus status)
        {
            Id = id;
            Status = status;
        }

        public long Id { get; }
        public AccountStatus Status { get; }
    }
}
