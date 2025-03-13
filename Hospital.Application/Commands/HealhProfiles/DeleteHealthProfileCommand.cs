using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthProfiles
{
    public class DeleteHealthProfileCommand : BaseCommand
    {
        public DeleteHealthProfileCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
