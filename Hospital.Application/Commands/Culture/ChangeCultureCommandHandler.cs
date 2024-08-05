using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Culture
{
    public class ChangeCultureCommandHandler : BaseCommandHandler, IRequestHandler<ChangeCultureCommand>
    {
        public ChangeCultureCommandHandler(IStringLocalizer<Resources> localizer) : base(localizer)
        {
        }

        public Task<Unit> Handle(ChangeCultureCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
