using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Auth.Actions
{
    public class GetActionsQueryHandler : BaseQueryHandler, IRequestHandler<GetActionsQuery, List<ActionDto>>
    {
        private readonly IActionReadRepository _actionReadRepository;

        public GetActionsQueryHandler(
            IAuthService authService,
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IActionReadRepository actionReadRepository
            ) : base(authService, mapper, localizer)
        {
            _actionReadRepository = actionReadRepository;

        }
        public async Task<List<ActionDto>> Handle(GetActionsQuery request, CancellationToken cancellationToken)
        {
            var actions = await _actionReadRepository.GetAsync(cancellationToken: cancellationToken);
            return _mapper.Map<List<ActionDto>>(actions);
        }
    }
}
