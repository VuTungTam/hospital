using AutoMapper;
using Hospital.Application.Dtos.Doctors;
using Hospital.Application.Repositories.Interfaces.Doctors;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
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
                request.Pagination, request.SpecialtyIds, request.State, request.Degree, request.Title, request.Rank, cancellationToken);

            var dtos = _mapper.Map<List<PublicDoctorDto>>(pagination.Data);

            return new PaginationResult<PublicDoctorDto>(dtos, pagination.Total);
        }
    }
}
