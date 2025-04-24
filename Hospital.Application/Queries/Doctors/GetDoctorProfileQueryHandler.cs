using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Doctors
{
    public class GetDoctorProfileQueryHandler : BaseQueryHandler, IRequestHandler<GetDoctorProfileQuery, DoctorDto>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        private readonly IExecutionContext _executionContext;
        public GetDoctorProfileQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IDoctorReadRepository doctorReadRepository,
            ISpecialtyReadRepository specialtyReadRepository,
            IExecutionContext executionContext,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
            _executionContext = executionContext;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<DoctorDto> Handle(GetDoctorProfileQuery request, CancellationToken cancellationToken)
        {
            var option = new QueryOption
            {
                Includes = new string[] { nameof(Doctor.DoctorSpecialties) }
            };
            var doctor = await _doctorReadRepository.GetByIdAsync(_executionContext.Identity, option, cancellationToken: cancellationToken);
            if (doctor == null)
            {
                throw new BadRequestException(_localizer["CommonMessage.DataWasDeletedOrNotPermission"]);
            }

            var speIds = doctor.DoctorSpecialties.Select(ds => ds.SpecialtyId).ToList();

            var specialties = await _specialtyReadRepository.GetByIdsAsync(speIds, cancellationToken: cancellationToken);

            var specialtyDtos = _mapper.Map<List<SpecialtyDto>>(specialties);

            var dto = _mapper.Map<DoctorDto>(doctor);

            dto.Specialties = specialtyDtos;

            dto.ProfessionalLevel = string.Join(" - ", new[]
            {
                dto.DoctorTitle > 0 ? _localizer["DoctorTitle."+dto.DoctorTitle.ToString()] : null,
                dto.DoctorDegree > 0 ? _localizer["DoctorDegree."+dto.DoctorDegree.ToString()] : null,
                dto.DoctorRank > 0 ? _localizer["DoctorRank."+dto.DoctorRank.ToString()] : null,
            }.Where(x => !string.IsNullOrEmpty(x))) ?? _localizer["Infor.None"];

            return dto;
        }
    }
}
