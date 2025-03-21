using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Auth.LoginHistories
{
    public class GetLoginHistoriesPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetLoginHistoriesPaginationQuery, PaginationResult<LoginHistoryDto>>
    {
        private readonly ILoginHistoryReadRepository _loginHistoryReadRepository;

        public GetLoginHistoriesPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ILoginHistoryReadRepository loginHistoryReadRepository
        ) : base(authService, mapper, localizer)
        {
            _loginHistoryReadRepository = loginHistoryReadRepository;
        }

        public async Task<PaginationResult<LoginHistoryDto>> Handle(GetLoginHistoriesPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _loginHistoryReadRepository.GetPaginationWithFilterAsync(request.Pagination, request.UserId, cancellationToken);
            var loginHistories = _mapper.Map<List<LoginHistoryDto>>(pagination.Data);

            return new PaginationResult<LoginHistoryDto>(loginHistories, pagination.Total);
        }
    }
}
