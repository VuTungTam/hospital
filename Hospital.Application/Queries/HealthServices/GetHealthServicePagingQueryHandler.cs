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
        private readonly IServiceTypeReadRepository _serviceTypeReadRepository;
        public GetHealthServicePagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthServiceReadRepository healthServiceReadRepository,
            IServiceTypeReadRepository serviceTypeReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthServiceReadRepository = healthServiceReadRepository;
            _serviceTypeReadRepository = serviceTypeReadRepository;
        }

        public async Task<PaginationResult<HealthServiceDto>> Handle(GetHealthServicePagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _healthServiceReadRepository.GetPagingWithFilterAsync(request.Pagination, request.Status, request.TypeId, request.FacilityId, request.SpecialtyId, cancellationToken: cancellationToken);

            var types = await _serviceTypeReadRepository.GetAsync(cancellationToken: cancellationToken);

            List<HealthServiceDto> dtos = new();

            foreach (var entity in result.Data)
            {
                var dto = _mapper.Map<HealthServiceDto>(entity);
                var type = types.First(x => x.Id.ToString() == dto.TypeId);
                dto.TypeNameVn = type.NameVn;
                dto.TypeNameEn = type.NameEn;
                dtos.Add(dto);
            }
            return new PaginationResult<HealthServiceDto>(dtos, result.Total);

        }
    }
}
