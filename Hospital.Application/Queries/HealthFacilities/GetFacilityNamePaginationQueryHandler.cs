using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetFacilityNamePaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetFacilityNamePaginationQuery, PaginationResult<FacilityNameDto>>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public GetFacilityNamePaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
        }

        public async Task<PaginationResult<FacilityNameDto>> Handle(GetFacilityNamePaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _healthFacilityReadRepository.GetNamePaginationAsync(request.Pagination, cancellationToken);

            var dtos = _mapper.Map<List<FacilityNameDto>>(data.Data);

            return new PaginationResult<FacilityNameDto>(dtos, data.Total);
        }
    }
}
