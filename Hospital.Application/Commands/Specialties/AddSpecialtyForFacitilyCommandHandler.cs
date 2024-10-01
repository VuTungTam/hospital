using Hospital.Application.Repositories.Interfaces.HealthFacilities;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.HealthFacilities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Specialties
{
    public class AddSpecialtyForFacitilyCommandHandler : BaseCommandHandler, IRequestHandler<AddSpecialtyForFacitilyCommand>
    {
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IHealthFacilityWriteRepository _healthFacilityWriteRepository;
        private readonly IHealthFacilityReadRepository _healthFacilityReadRepository;
        public AddSpecialtyForFacitilyCommandHandler(
            IStringLocalizer<Resources> localizer,
            ISpecialtyReadRepository specialtyReadRepository,
            IHealthFacilityWriteRepository healthfacilityWriteRepository,
            IHealthFacilityReadRepository healthfacilityReadRepository
            ) : base(localizer)
        {
            _healthFacilityWriteRepository = healthfacilityWriteRepository;
            _specialtyReadRepository = specialtyReadRepository;
            _healthFacilityReadRepository = healthfacilityReadRepository;
        }

        public async Task<Unit> Handle(AddSpecialtyForFacitilyCommand request, CancellationToken cancellationToken)
        {
            if(request.FacilityId <= 0)
            {
                throw new BadRequestException("Cơ sở không hợp lệ");
            }

            if (request.SpecialtyId <= 0)
            {
                throw new BadRequestException("Chuyên khoa không hợp lệ");
            }
            //var includes = new string[] { nameof(HealthFacility.FacilitySpecialties) };
            var includes = new string[] {};
            //var facility = await _healthFacilityReadRepository.GetByIdAsync(request.FacilityId, includes, cancellationToken);
            //if (facility == null)
            //{
            //    throw new BadRequestException("Cơ sở không tồn tại");
            //}

            //facility.FacilitySpecialties ??= new();

            //if(facility.FacilitySpecialties.Exists(x => x.SpecialtyId == request.SpecialtyId))
            //{
            //    throw new BadRequestException("Chuyên khoa đã tồn tại");
            //}

            //var specialty = await _specialtyReadRepository.GetByIdAsync(request.SpecialtyId, cancellationToken: cancellationToken);
            //if (specialty == null)
            //{
            //    throw new BadRequestException("Chuyên khoa không tồn tại");
            //}
            //facility.FacilitySpecialties.Add(new FacilitySpecialty { FacilityId = request.FacilityId, SpecialtyId = request.SpecialtyId });
            //await _healthFacilityWriteRepository.UpdateAsync(facility, cancellationToken: cancellationToken);
            return Unit.Value;
        }
    }
}
