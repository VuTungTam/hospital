using AutoMapper;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetHealthProfileByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthProfileByIdQuery, HealthProfileDto>
    {
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        private readonly IExecutionContext _executionContext;
        public GetHealthProfileByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IHealthProfileReadRepository healthProfileReadRepository,
            IExecutionContext executionContext,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _healthProfileReadRepository = healthProfileReadRepository;
            _executionContext = executionContext;
        }

        public async Task<HealthProfileDto> Handle(GetHealthProfileByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            HealthProfile profile;
            if (_executionContext.AccountType == AccountType.Customer)
            {
                profile = await _healthProfileReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            }
            else
            {
                var option = new QueryOption()
                {
                    IgnoreOwner = true
                };
                profile = await _healthProfileReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);

            }


            if (profile == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var profileDto = _mapper.Map<HealthProfileDto>(profile);

            return profileDto;
        }
    }
}
