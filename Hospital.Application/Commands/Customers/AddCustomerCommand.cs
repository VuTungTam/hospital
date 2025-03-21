using Hospital.Application.Dtos.Customers;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Customers
{
    public class AddCustomerCommand : BaseCommand<string>
    {
        public AddCustomerCommand(CustomerDto customer)
        {
            Customer = customer;
        }
        public CustomerDto Customer { get;}
    }
}
