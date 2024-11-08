using AutoMapper;
using Hospital.Application.Dtos.Branches;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Branches
{
    public class GetBranchByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetBranchByIdQuery, BranchDto>
    {
        private readonly IBranchReadRepository _branchReadRepository;

        public GetBranchByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IBranchReadRepository branchReadRepository
        ) : base(authService, mapper, localizer)
        {
            _branchReadRepository = branchReadRepository;
        }

        public async Task<BranchDto> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }
            var branch = await _branchReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            return _mapper.Map<BranchDto>(branch);
        }
    }
}
