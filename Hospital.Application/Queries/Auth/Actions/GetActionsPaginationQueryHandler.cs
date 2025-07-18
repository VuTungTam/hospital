﻿using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Auth.Actions
{
    public class GetActionsPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetActionsPaginationQuery, PaginationResult<ActionDto>>
    {
        private readonly IActionReadRepository _actionReadRepository;

        public GetActionsPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IActionReadRepository actionReadRepository
        ) : base(authService, mapper, localizer)
        {
            _actionReadRepository = actionReadRepository;
        }

        public async Task<PaginationResult<ActionDto>> Handle(GetActionsPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _actionReadRepository.GetPaginationAsync(request.Pagination, spec:null, cancellationToken: cancellationToken);
            var dtos = _mapper.Map<List<ActionDto>>(pagination.Data);

            return new PaginationResult<ActionDto>(dtos, pagination.Total);
        }
    }
}
