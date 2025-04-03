using AutoMapper;
using Hospital.Application.Dtos.Symptoms;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Symptoms
{
    public class GetSymptomPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetSymptomPagingQuery, PaginationResult<SymptomDto>>
    {
        private readonly ISymptomReadRepository _symptomReadRepository;

        public GetSymptomPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ISymptomReadRepository symptomReadRepository
            ) : base(authService, mapper, localizer)
        {
            _symptomReadRepository = symptomReadRepository;
        }

        public async Task<PaginationResult<SymptomDto>> Handle(GetSymptomPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _symptomReadRepository.GetPaginationAsync(request.Pagination,spec:null, _symptomReadRepository.DefaultQueryOption ,cancellationToken);

            var symptoms = _mapper.Map<List<SymptomDto>>(paging.Data);

            return new PaginationResult<SymptomDto>(symptoms, paging.Total);
        }
    }
}
