using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServicePagingQueryHandler : BaseQueryHandler, IRequestHandler<GetHealthServicePagingQuery, PaginationResult<HealthServiceDto>>
    {
        private readonly IHealthServiceReadRepository _healthServiceReadRepository;
        public GetHealthServicePagingQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
        }

        public async Task<PaginationResult<HealthServiceDto>> Handle(GetHealthServicePagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _healthServiceReadRepository.GetPagingWithFilterAsync(request.Pagination, request.Status, request.TypeId, request.FacilityId, request.SpecialtyId, cancellationToken: cancellationToken);

            var dtos = _mapper.Map<List<HealthServiceDto>>(result.Data);

            return new PaginationResult<HealthServiceDto>(dtos, result.Total);

        }
    }
}
