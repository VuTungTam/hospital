using AutoMapper;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Users
{
    public class CheckAccessByPasswordQueryHandler : BaseQueryHandler, IRequestHandler<CheckAccessByPasswordQuery, string>
    {
        public CheckAccessByPasswordQueryHandler(
            IMapper mapper, 
            IStringLocalizer<Resources> localizer
            ) : base(mapper, localizer)
        {
        }

        public Task<string> Handle(CheckAccessByPasswordQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
