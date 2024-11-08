using Hospital.Application.Dtos.Newes;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Newes
{
    [RequiredPermission(ActionExponent.UIManagement)]
    public class UpdateNewsCommand : BaseCommand
    {
        public UpdateNewsCommand(NewsDto news)
        {
            News = news;
        }

        public NewsDto News { get; }
    }
}
