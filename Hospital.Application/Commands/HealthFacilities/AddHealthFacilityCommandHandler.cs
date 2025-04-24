using AutoMapper;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Images;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthFacilities
{
    public class AddHealthFacilityCommandHandler : BaseCommandHandler, IRequestHandler<AddHealthFacilityCommand, string>
    {
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IFacilityTypeReadRepository _facilityTypeReadRepository;

        public AddHealthFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IHealthFacilityWriteRepository healthFacilityWriteRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IFacilityTypeReadRepository facilityTypeReadRepository,
            IMapper mapper
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
            _facilityTypeReadRepository = facilityTypeReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<string> Handle(AddHealthFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = _mapper.Map<HealthFacility>(request.HealthFacility);

            var speIds = request.HealthFacility.SpecialtyIds.Select(x => long.Parse(x)).ToList();

            var specialties = await _specialtyReadRepository.GetByIdsAsync(speIds, cancellationToken: cancellationToken);

            facility.FacilitySpecialties = new();

            foreach (var id in speIds)
            {
                facility.FacilitySpecialties.Add(new FacilitySpecialty
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    SpecialtyId = id,
                });
            }

            var typeIds = request.HealthFacility.TypeIds.Select(x => long.Parse(x)).ToList();

            var types = await _facilityTypeReadRepository.GetByIdsAsync(typeIds, cancellationToken: cancellationToken);

            facility.FacilityTypeMappings = new();

            foreach (var id in typeIds)
            {
                facility.FacilityTypeMappings.Add(new FacilityTypeMapping
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    TypeId = id,
                });
            }

            facility.Images = new();

            foreach (var imageName in request.HealthFacility.ImageNames)
            {
                facility.Images.Add(new Image
                {
                    Id = AuthUtility.GenerateSnowflakeId(),
                    PublicId = imageName,
                });
            }

            await _healthFacilityWriteRepository.AddAsync(facility, cancellationToken);

            return facility.Id.ToString();
        }
    }
}
