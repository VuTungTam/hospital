using AutoMapper;
using Hospital.Application.Dtos.Branches;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Branches
{
    public class GetBranchesPagingQueryHandler : BaseQueryHandler, IRequestHandler<GetBranchesPagingQuery, PagingResult<BranchDto>>
    {
        private readonly IBranchReadRepository _branchReadRepository;

        public GetBranchesPagingQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBranchReadRepository branchReadRepository
        ) : base(authService, mapper, localizer)
        {
            _branchReadRepository = branchReadRepository;
        }

        public async Task<PagingResult<BranchDto>> Handle(GetBranchesPagingQuery request, CancellationToken cancellationToken)
        {
            var paging = await _branchReadRepository.GetPagingWithFilterAsync(request.Pagination, request.Status, cancellationToken);
            var branches = _mapper.Map<List<BranchDto>>(paging.Data);
            return new PagingResult<BranchDto>(branches, paging.Total);
        }
    }
}
