using AutoMapper;
using Hospital.Application.Dtos.HealthProfiles;
using Hospital.Application.Repositories.Interfaces.HealthProfiles;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetHealthProfilePagingQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthProfilePagingQuery, PagingResult<HealthProfileDto>>
    {
        private readonly IHealthProfileReadRepository _healthProfileReadRepository;
        public GetHealthProfilePagingQueryHandler(
            IAuthService authService, 
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthProfileReadRepository healthProfileReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthProfileReadRepository = healthProfileReadRepository;
        }

        public async Task<PagingResult<HealthProfileDto>> Handle(GetHealthProfilePagingQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _healthProfileReadRepository.GetPagingWithFilterAsync(request.Pagination, request.UserId, cancellationToken);   
            
            var profileDtos = _mapper.Map<List<HealthProfileDto>>(profiles.Data); 
            
            return new PagingResult<HealthProfileDto> (profileDtos, profiles.Total );
        }
    }
}
