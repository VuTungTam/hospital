using AutoMapper;
using Hospital.Application.Dtos.Branches;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Branches
{
    public class GetBranchesQueryHandler : BaseQueryHandler, IRequestHandler<GetBranchesQuery, List<BranchDto>>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IBranchReadRepository _branchReadRepository;

        public GetBranchesQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IExecutionContext executionContext,
            IBranchReadRepository branchReadRepository
        ) : base(authService, mapper, localizer)
        {
            _executionContext = executionContext;
            _branchReadRepository = branchReadRepository;
        }

        public async Task<List<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
        {
            var branches = await _branchReadRepository.GetAsync(cancellationToken: cancellationToken);
            var result = _mapper.Map<List<BranchDto>>(branches);

            foreach (var branch in result)
            {
                branch.HasPermission = _executionContext.IsSuperAdmin() || _executionContext.BranchIds.Contains(long.Parse(branch.Id));
            }

            return result;
        }
    }
}
