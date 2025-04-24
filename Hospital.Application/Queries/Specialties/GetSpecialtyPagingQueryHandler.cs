using AutoMapper;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Repositories.Interfaces.Specialities;
using Hospital.Domain.Entities.Specialties;
using Hospital.Domain.Specifications.Specialties;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Specifications;
using Hospital.SharedKernel.Specifications.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Specialties
{
    public class GetSpecialtyPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetSpecialtyPagingQuery, PaginationResult<SpecialtyDto>>
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

        public async Task<PaginationResult<SpecialtyDto>> Handle(GetSpecialtyPagingQuery request, CancellationToken cancellationToken)
        {
            var spec = new ExpressionSpecification<Specialty>(x => true);
            QueryOption option = new();
            if (request.FacilityId > 0)
            {
                spec = new GetSpecialtiesByFacilityIdSpecification(request.FacilityId);
                option.Includes = new string[] { nameof(Specialty.FacilitySpecialties) };
            }

            var result = await _specialtyReadRepository.GetPaginationAsync(request.Pagination, spec, option, cancellationToken: cancellationToken);

            var specialties = _mapper.Map<List<SpecialtyDto>>(result.Data);

            return new PaginationResult<SpecialtyDto>(specialties, result.Total);
        }
    }
}
