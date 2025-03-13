using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Specialties
{
    public class RemoveSpecialtyForFacilityCommandHandler : BaseCommandHandler, IRequestHandler<RemoveSpecialtyForFacilityCommand>
    {
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        public RemoveSpecialtyForFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            ISpecialtyReadRepository specialtyReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IHealthFacilityWriteRepository healthFacilityWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<Unit> Handle(RemoveSpecialtyForFacilityCommand request, CancellationToken cancellationToken)
        {
            if (request.SpecialtyId <= 0)
            {
                throw new BadRequestException("Chuyen khoa không hợp lệ");
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthFacility.FacilitySpecialties) }
            };
            var facility = await _healthFacilityReadRepository.GetByIdAsync(request.FacilityId, option, cancellationToken: cancellationToken);
            if (facility == null)
            {
                throw new BadRequestException("Chi nhanh khong ton tai");
            }
            if (facility.FacilitySpecialties == null || !facility.FacilitySpecialties.Exists(x => x.SpecialtyId == request.SpecialtyId))
            {
                throw new BadRequestException("Chi nhanh khong co chuyen khoa này");
            }
            var specialty = await _specialtyReadRepository.GetByIdAsync(request.SpecialtyId, _specialtyReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (specialty == null)
            {
                throw new BadRequestException("Chuyen khoa khong ton tai");
            }
            await _healthFacilityWriteRepository.RemoveFacilitySpecialtyAsync(facility.FacilitySpecialties.First(x => x.SpecialtyId == request.SpecialtyId), cancellationToken);

            return Unit.Value;
        }
    }
}
