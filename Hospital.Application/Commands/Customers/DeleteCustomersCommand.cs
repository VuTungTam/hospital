using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using MediatR;

namespace Hospital.Application.Commands.Customers
{
    public class DeleteCustomersCommand : BaseCommand
    {
        public DeleteCustomersCommand(List<long> ids) 
        {
            Ids = ids;
        }
        public List<long> Ids { get;}
    }
}
