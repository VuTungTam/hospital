using AutoMapper;
using Hospital.Application.Dtos.HealthFacility;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetFacilityByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetFacilityByIdQuery, HealthFacilityDto>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IFacilityTypeReadRepository _facilityTypeReadRepository;
        public GetFacilityByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IFacilityTypeReadRepository facilityTypeReadRepository
            ) : base(authService, mapper, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _facilityTypeReadRepository = facilityTypeReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<HealthFacilityDto> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthFacility.Images), nameof(HealthFacility.FacilitySpecialties), nameof(HealthFacility.FacilityTypeMappings) }
            };

            var facility = await _healthFacilityReadRepository.GetByIdAsync(request.Id, option, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var facilityDto = _mapper.Map<HealthFacilityDto>(facility);

            var speIds = facility.FacilitySpecialties.Select(ds => ds.SpecialtyId).ToList();

            var specialties = await _specialtyReadRepository.GetByIdsAsync(speIds, cancellationToken: cancellationToken);

            facilityDto.Specialties = _mapper.Map<List<SpecialtyDto>>(specialties);

            var typeIds = facility.FacilityTypeMappings.Select(ds => ds.TypeId).ToList();

            var types = await _facilityTypeReadRepository.GetByIdsAsync(typeIds, cancellationToken: cancellationToken);

            facilityDto.Types = _mapper.Map<List<FacilityTypeDto>>(types);

            var imageNames = new List<string>();

            foreach (var image in facility.Images)
            {
                var imageName = image.PublicId.Split('/').Last();
                imageNames.Add(imageName);
            }
            facilityDto.ImageNames = imageNames;

            return facilityDto;
        }
    }
}
