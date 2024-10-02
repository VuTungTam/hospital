using Hospital.Application.Dtos.Visits;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Visits
{
    public class AddVisitCommand : BaseCommand<long>
    {
        public AddVisitCommand( VisitDto dto)
        {
            Dto = dto;
        }
        public VisitDto Dto { get; set; }
    }
}
