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
    public class GetDoctorsPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetDoctorsPaginationQuery, PaginationResult<DoctorDto>>
    {
        private readonly IDoctorReadRepository _doctorReadRepository;
        public GetDoctorsPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper, 
            IDoctorReadRepository doctorReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _doctorReadRepository = doctorReadRepository;
        }

        public async Task<PaginationResult<DoctorDto>> Handle(GetDoctorsPaginationQuery request, CancellationToken cancellationToken)
        {

            var pagination = await _doctorReadRepository.GetDoctorsPaginationResultAsync(
                request.Pagination, request.SpecialtyIds, request.State, request.Degree, request.Title, request.Rank, cancellationToken);
            var dtos = _mapper.Map<List<DoctorDto>>(pagination.Data);

            return new PaginationResult<DoctorDto>(dtos, pagination.Total);
        }
    }
}
