using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Branches
{
    public class AddBranchCommandHandler : BaseCommandHandler, IRequestHandler<AddBranchCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IBranchWriteRepository _branchWriteRepository;

        public AddBranchCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBranchWriteRepository branchWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _branchWriteRepository = branchWriteRepository;
        }

        public async Task<string> Handle(AddBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = _mapper.Map<Branch>(request.Branch);

            await _branchWriteRepository.AddAsync(branch, cancellationToken);

            return branch.Id.ToString();
        }
    }
}
