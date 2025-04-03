using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.Roles
{
    public class AddActionForRoleCommandHandler : BaseCommandHandler, IRequestHandler<AddActionForRoleCommand>
    {
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly IRoleWriteRepository _roleWriteRepository;
        private readonly IActionReadRepository _actionReadRepository;

        public AddActionForRoleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IRoleReadRepository roleReadRepository,
            IRoleWriteRepository roleWriteRepository,
            IActionReadRepository actionReadRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _roleReadRepository = roleReadRepository;
            _roleWriteRepository = roleWriteRepository;
            _actionReadRepository = actionReadRepository;
        }

        public async Task<Unit> Handle(AddActionForRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.RoleId <= 0)
            {
                throw new BadRequestException("Vai trò không hợp lệ");
            }

            if (request.ActionId <= 0)
            {
                throw new BadRequestException("Chức năng không hợp lệ");
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(Role.RoleActions) }
            };
            var role = await _roleReadRepository.GetByIdAsync(request.RoleId, option, cancellationToken);
            if (role == null)
            {
                throw new BadRequestException("Vai trò không tồn tại");
            }

            if (role.Code == RoleCodeConstant.SUPER_ADMIN)
            {
                throw new BadRequestException("Không thể cập nhật vai trò này");
            }

            role.RoleActions ??= new();
            if (role.RoleActions.Exists(x => x.ActionId == request.ActionId))
            {
                throw new BadRequestException("Chức năng đã tồn tại");
            }

            var masterAction = await _actionReadRepository.GetMasterAsync(cancellationToken);
            if (masterAction.Id == request.ActionId)
            {
                throw new BadRequestException("Không thể thêm chức năng này");
            }

            var action = await _actionReadRepository.GetByIdAsync(request.ActionId, _actionReadRepository.DefaultQueryOption, cancellationToken: cancellationToken);
            if (action == null)
            {
                throw new BadRequestException("Chức năng không tồn tại");
            }

            role.RoleActions.Add(new RoleAction { Id = AuthUtility.GenerateSnowflakeId(), ActionId = request.ActionId, RoleId = request.RoleId });

            await _roleWriteRepository.UpdateAsync(role, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
