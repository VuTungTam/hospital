using AutoMapper;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyByDoctorIdQueryHandler : BaseQueryHandler, IRequestHandler<GetSpecialtyByDoctorIdQuery, List<SpecialtyDto>>
    {
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IDoctorReadRepository _doctorReadRepository;
        public GetSpecialtyByDoctorIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ISpecialtyReadRepository specialtyReadRepository,
            IDoctorReadRepository doctorReadRepository
            ) : base(authService, mapper, localizer)
        {
            _specialtyReadRepository = specialtyReadRepository;
            _doctorReadRepository = doctorReadRepository;
        }

        public async Task<List<SpecialtyDto>> Handle(GetSpecialtyByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            if (request.DoctorId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }
            var doctor = await _doctorReadRepository.GetPublicDoctorById(request.DoctorId, cancellationToken);

            var ids = doctor.DoctorSpecialties.Select(x => x.SpecialtyId).ToList();

            var result = await _specialtyReadRepository.GetByIdsAsync(ids, cancellationToken: cancellationToken);

            if (result == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            return _mapper.Map<List<SpecialtyDto>>(result);
        }
    }
}
