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
    public class GetSymptomPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetSymptomPagingQuery, PagingResult<SymptomDto>>
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

        public async Task<PagingResult<SymptomDto>> Handle(GetSymptomPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _symptomReadRepository.GetPagingAsync(request.Pagination);

            var symptoms = _mapper.Map<List<SymptomDto>>(paging.Data);

            return new PagingResult<SymptomDto>(symptoms, paging.Total);
        }
    }
}
