using AutoMapper;
using Hospital.Application.Dtos.HealthServices;
using Hospital.Application.Dtos.ServiceTimeRules;
using Hospital.Application.Repositories.Interfaces.HealthServices;
using Hospital.Application.Repositories.Interfaces.ServiceTimeRules;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.ServiceTimeRules
{
    public class GetServiceTimeRulePagingQueryHandler : BaseQueryHandler, IRequestHandler<GetServiceTimeRulePagingQuery, PaginationResult<ServiceTimeRuleDto>>
    {
        private readonly IServiceTimeRuleReadRepository _serviceTimeRuleReadRepository;
        public GetServiceTimeRulePagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IServiceTimeRuleReadRepository serviceTimeRuleReadRepository
            ) : base(authService, mapper, localizer)
        {
            _serviceTimeRuleReadRepository = serviceTimeRuleReadRepository;
        }

        public async Task<PaginationResult<ServiceTimeRuleDto>> Handle(GetServiceTimeRulePagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _serviceTimeRuleReadRepository.GetPagingWithFilterAsync(request.Pagination, request.ServiceId, request.DayOfWeek, cancellationToken: cancellationToken);

            var dtos = _mapper.Map<List<ServiceTimeRuleDto>>(result.Data);

            return new PaginationResult<ServiceTimeRuleDto>(dtos, result.Total);
        }
    }
}

