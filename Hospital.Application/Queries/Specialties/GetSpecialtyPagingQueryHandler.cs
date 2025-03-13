using AutoMapper;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Specifications.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Specifications.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetSpecialtyPagingQuery, PagingResult<SpecialtyDto>>
    {
        private readonly ISpecialtyReadRepository _specialtyReadRepository; 
        public GetSpecialtyPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ISpecialtyReadRepository specialtyReadRepository
            ) : base(authService, mapper, localizer)
        {
            _specialtyReadRepository = specialtyReadRepository;
        }

        public async Task<PagingResult<SpecialtyDto>> Handle(GetSpecialtyPagingQuery request, CancellationToken cancellationToken)
        {
            ISpecification<Specialty> spec = null;
            if (request.FacilityId > 0)
            {
                spec = new GetSpecialtiesByFacilityIdSpecification(request.FacilityId);
            }

            var result = await _specialtyReadRepository.GetPagingAsync(request.Pagination, spec, _specialtyReadRepository.DefaultQueryOption, cancellationToken);

            var specialties = _mapper.Map<List<SpecialtyDto>>(result.Data);

            return new PagingResult<SpecialtyDto>(specialties, result.Total);
        }
    }
}
