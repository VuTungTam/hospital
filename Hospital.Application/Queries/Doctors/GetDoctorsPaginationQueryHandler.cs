using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Doctors
{
    public class GetDoctorsPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetDoctorsPaginationQuery, PaginationResult<DoctorDto>>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        private readonly ISpecialtyReadRepository _specialtyReadRepository;
        public GetDoctorsPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IDoctorReadRepository doctorReadRepository,
            ISpecialtyReadRepository specialtyReadRepository
        ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<PaginationResult<DoctorDto>> Handle(GetDoctorsPaginationQuery request, CancellationToken cancellationToken)
        {
            var filterRequest = request.Request;
            var pagination = await _doctorReadRepository.GetDoctorsPaginationResultAsync(
                request.Pagination, request.Request, request.State, cancellationToken);

            var doctors = pagination.Data;
            List<DoctorDto> dtos = new();
            foreach (var doctor in doctors)
            {
                var dto = _mapper.Map<DoctorDto>(doctor);
                var specialties = doctor.DoctorSpecialties.Select(x => x.Specialty).ToList();
                var specialtyDtos = _mapper.Map<List<SpecialtyDto>>(specialties);
                dto.Specialties = specialtyDtos;
                dto.ProfessionalLevel = string.Join(" - ", new[]
                {
                    dto.DoctorTitle > 0 ? _localizer["DoctorTitle."+dto.DoctorTitle.ToString()] : null,
                    dto.DoctorDegree > 0 ? _localizer["DoctorDegree."+dto.DoctorDegree.ToString()] : null,
                    dto.DoctorRank > 0 ? _localizer["DoctorRank."+dto.DoctorRank.ToString()] : null,
                }.Where(x => !string.IsNullOrEmpty(x))) ?? _localizer["Infor.None"];
                dtos.Add(dto);
            }

            return new PaginationResult<DoctorDto>(dtos, pagination.Total);
        }
    }
}
