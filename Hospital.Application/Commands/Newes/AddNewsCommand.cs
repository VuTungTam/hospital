using Hospital.Application.Dtos.Newes;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Newes
{
    [RequiredPermission(ActionExponent.UIManagement)]
    public class AddNewsCommand : BaseCommand<string>
    {
        public AddNewsCommand(NewsDto news)
        {
            News = news;
        }

        public NewsDto News { get; }
    }
}
