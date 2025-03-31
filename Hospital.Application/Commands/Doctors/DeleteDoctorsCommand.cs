using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Doctors
{
    public class DeleteDoctorsCommand : BaseCommand
    {
        public DeleteDoctorsCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
