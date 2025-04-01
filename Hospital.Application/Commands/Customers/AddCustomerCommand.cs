using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Customers
{
    [RequiredPermission(ActionExponent.AddCustomer)]

    public class AddCustomerCommand : BaseCommand<string>
    {
        public AddCustomerCommand(CustomerDto customer)
        {
            Customer = customer;
        }
        public CustomerDto Customer { get;}
    }
}
