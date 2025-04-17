using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Base;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.FacilityTypes
{
    public class GetFacilityTypeQueryHandler : BaseQueryHandler, IRequestHandler<GetFacilityTypeQuery, List<FacilityTypeDto>>
    {
        private readonly IFacilityTypeReadRepository _facilityCategoryReadRepository;
        public GetFacilityTypeQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IFacilityTypeReadRepository facilityTypeReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _facilityCategoryReadRepository = facilityTypeReadRepository;
        }

        public async Task<List<FacilityTypeDto>> Handle(GetFacilityTypeQuery request, CancellationToken cancellationToken)
        {
            var tuples = await _facilityCategoryReadRepository.GetTypeAsync(cancellationToken);

            var result = tuples
                 .Select(x =>
                 {
                     var dto = _mapper.Map<FacilityTypeDto>(x.Type);
                     dto.Total = x.Total;
                     return dto;
                 })
                 .ToList();
            return result;
        }
    }
}
