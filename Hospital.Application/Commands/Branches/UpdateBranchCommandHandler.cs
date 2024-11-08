using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Branches;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Branches;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Branches
{
    public class UpdateBranchCommandHandler : BaseCommandHandler, IRequestHandler<UpdateBranchCommand>
    {
        private readonly IMapper _mapper;
        private readonly IBranchReadRepository _branchReadRepository;
        private readonly IBranchWriteRepository _branchWriteRepository;

        public UpdateBranchCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBranchReadRepository branchReadRepository,
            IBranchWriteRepository branchWriteRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _mapper = mapper;
            _branchReadRepository = branchReadRepository;
            _branchWriteRepository = branchWriteRepository;
        }

        public async Task<Unit> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            if (!long.TryParse(request.Branch.Id, out var id) || id <= 0)
            {
                throw new BadRequestException(_localizer["common_id_is_not_valid"]);
            }

            var branch = await _branchReadRepository.GetByIdAsync(id, cancellationToken: cancellationToken);
            if (branch == null)
            {
                throw new BadRequestException(_localizer["common_data_does_not_exist_or_was_deleted"]);
            }

            var entity = _mapper.Map<Branch>(request.Branch);

            await _branchWriteRepository.UpdateAsync(entity, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
