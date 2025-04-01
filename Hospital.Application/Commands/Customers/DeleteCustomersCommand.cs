using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Customers
{
    [RequiredPermission(ActionExponent.DeleteCustomer)]

    public class DeleteCustomersCommand : BaseCommand
    {
        public DeleteCustomersCommand(List<long> ids) 
        {
            Ids = ids;
        }
        public List<long> Ids { get;}
    }
}
