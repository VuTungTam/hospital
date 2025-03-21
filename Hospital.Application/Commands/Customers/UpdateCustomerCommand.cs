using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Customers
{
    //[RequiredPermission(ActionExponent.UpdateCustomer)]
    public class UpdateCustomerCommand : BaseCommand
    {
        public UpdateCustomerCommand(CustomerDto customer)
        {
            Customer = customer;
        }

        public CustomerDto Customer { get; }
    }
}
