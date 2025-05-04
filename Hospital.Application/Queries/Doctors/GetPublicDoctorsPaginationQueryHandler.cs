using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Doctors
{
    public class GetPublicDoctorsPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetPublicDoctorsPaginationQuery, PaginationResult<PublicDoctorDto>>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        public GetPublicDoctorsPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IDoctorReadRepository doctorReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
        }

        public async Task<PaginationResult<PublicDoctorDto>> Handle(GetPublicDoctorsPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _doctorReadRepository.GetPublicDoctorsPaginationResultAsync(
                request.Pagination, request.Request, request.FacilityId, request.State, cancellationToken);

            var dtos = _mapper.Map<List<PublicDoctorDto>>(pagination.Data);

            dtos.ForEach(dto =>
            {
                dto.ProfessionalLevel = string.Join(" - ", new[]
                {
                    dto.DoctorTitle > 0 ? _localizer["DoctorTitle."+dto.DoctorTitle.ToString()] : null,
                    dto.DoctorDegree > 0 ? _localizer["DoctorDegree."+dto.DoctorDegree.ToString()] : null,
                    dto.DoctorRank > 0 ? _localizer["DoctorRank."+dto.DoctorRank.ToString()] : null,
                }.Where(x => !string.IsNullOrEmpty(x))) ?? _localizer["Infor.None"];

                dto.Credentials = string.Join(" ", new[]
                {
                    dto.DoctorTitle > 0 ? dto.DoctorTitle.ToString() : null,
                    dto.DoctorDegree > 0 ? dto.DoctorDegree.ToString() : null,
                    dto.DoctorRank > 0 ? dto.DoctorRank.ToString() : null,
                }.Where(x => !string.IsNullOrEmpty(x))) ?? _localizer["Infor.None"];
            });

            return new PaginationResult<PublicDoctorDto>(dtos, pagination.Total);
        }
    }
}
