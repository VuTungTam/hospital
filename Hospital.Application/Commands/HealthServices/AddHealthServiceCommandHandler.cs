using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Domain.Entities.Doctors;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.HealthServices
{
    public class AddHealthServiceCommandHandler : BaseCommandHandler, IRequestHandler<AddHealthServiceCommand, string>
    {

        private readonly IHealthServiceWriteRepository _healthServiceWriteRepository;
        private readonly IExecutionContext _executionContext;
        private readonly IHealthFacilityReadRepository _facilityReadRepository;
        private readonly IDoctorReadRepository _doctorReadRepository;
        public AddHealthServiceCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IHealthFacilityReadRepository facilityReadRepository,
            IDoctorReadRepository doctorReadRepository,
            IExecutionContext executionContext,
            IHealthServiceWriteRepository healthServiceWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _healthServiceWriteRepository = healthServiceWriteRepository;
            _executionContext = executionContext;
            _facilityReadRepository = facilityReadRepository;
            _doctorReadRepository = doctorReadRepository;
        }

        public async Task<string> Handle(AddHealthServiceCommand request, CancellationToken cancellationToken)
        {
            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthFacility.FacilitySpecialties) }
            };

            var facility = await _facilityReadRepository.GetByIdAsync(_executionContext.FacilityId, option: option, cancellationToken: cancellationToken);

            if (facility == null)
            {
                throw new BadRequestException("Loi");
            }

            var specialtyIds = facility.FacilitySpecialties.Select(x => x.SpecialtyId.ToString()).ToList();

            if (!specialtyIds.Contains(request.HealthService.SpecialtyId))
            {
                throw new BadRequestException("Loi");
            }

            var option2 = new QueryOption
            {
                Includes = new string[] { nameof(Doctor.DoctorSpecialties) }
            };

            var doctorId = long.Parse(request.HealthService.DoctorId);

            var doctor = await _doctorReadRepository.GetByIdAsync(doctorId, option2, cancellationToken: cancellationToken);

            var doctorSpeIds = doctor.DoctorSpecialties.Select(x => x.SpecialtyId.ToString()).ToList();

            if (!doctorSpeIds.Contains(request.HealthService.SpecialtyId))
            {
                throw new BadRequestException("Loi");
            }

            var service = _mapper.Map<HealthService>(request.HealthService);

            service.FacilityId = _executionContext.FacilityId;

            service.SpecialtyId = long.Parse(request.HealthService.SpecialtyId);

            await _healthServiceWriteRepository.AddAsync(service, cancellationToken);

            return service.Id.ToString();
        }
    }
}
