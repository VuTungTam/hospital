using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Specialties;
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
    public class AddSpecialtyForFacilityCommandHandler : BaseCommandHandler, IRequestHandler<AddSpecialtyForFacilityCommand>
    {
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        public AddSpecialtyForFacilityCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            ISpecialtyReadRepository specialtyReadRepository,
            IHealthFacilityReadRepository healthFacilityReadRepository,
            IHealthFacilityWriteRepository healthFacilityWriteRepository
            ) : base(eventDispatcher, authService, localizer)
        {
            _specialtyReadRepository = specialtyReadRepository;
            _healthFacilityReadRepository = healthFacilityReadRepository;
            _healthFacilityWriteRepository = healthFacilityWriteRepository;
        }

        public async Task<Unit> Handle(AddSpecialtyForFacilityCommand request, CancellationToken cancellationToken)
        {
            if (request.FacilityId <= 0)
            {
                throw new BadRequestException("Cơ sở không hợp lệ");
            }

            if (request.SpecialtyId <= 0)
            {
                throw new BadRequestException("Chuyên khoa không hợp lệ");
            }
            var option = new QueryOption
            {
                Includes = new string[] { nameof(HealthFacility.FacilitySpecialties) }
            };
            var facility = await _healthFacilityReadRepository.GetByIdAsync(request.FacilityId, option, cancellationToken: cancellationToken);
            if (facility == null)
            {
                throw new BadRequestException("Co so không tồn tại");
            }

            facility.FacilitySpecialties ??= new();

            if (facility.FacilitySpecialties.Exists(x => x.SpecialtyId == request.SpecialtyId))
            {
                throw new BadRequestException("Chuyên khoa đã tồn tại");
            }

            var specialty = await _specialtyReadRepository.GetByIdAsync(request.SpecialtyId,_specialtyReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (specialty == null)
            {
                throw new BadRequestException("Chuyên khoa không tồn tại");
            }
            facility.FacilitySpecialties.Add(new FacilitySpecialty { FacilityId = request.FacilityId, SpecialtyId = request.SpecialtyId });
            await _healthFacilityWriteRepository.UpdateAsync(facility, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
