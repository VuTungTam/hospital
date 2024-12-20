﻿using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Auth.Roles
{
    public class RemoveActionForRoleCommandHandler : BaseCommandHandler, IRequestHandler<RemoveActionForRoleCommand>
    {
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly IRoleWriteRepository _roleWriteRepository;
        private readonly IActionReadRepository _actionReadRepository;

        public RemoveActionForRoleCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IRoleReadRepository roleReadRepository,
            IRoleWriteRepository roleWriteRepository,
            IActionReadRepository actionReadRepository
        ) : base(eventDispatcher, authService, localizer)
        {
            _roleReadRepository = roleReadRepository;
            _roleWriteRepository = roleWriteRepository;
            _actionReadRepository = actionReadRepository;
        }

        public async Task<Unit> Handle(RemoveActionForRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.RoleId <= 0)
            {
                throw new BadRequestException("Vai trò không hợp lệ");
            }

            if (request.ActionId <= 0)
            {
                throw new BadRequestException("Chức năng không hợp lệ");
            }

            var includes = new string[] { nameof(Role.RoleActions) };
            var role = await _roleReadRepository.GetByIdAsync(request.RoleId, includes, cancellationToken: cancellationToken);
            if (role == null)
            {
                throw new BadRequestException("Vai trò không tồn tại");
            }

            if (role.Code == RoleCodeConstant.SUPER_ADMIN)
            {
                throw new BadRequestException("Không thể cập nhật vai trò này");
            }

            if (role.RoleActions == null || !role.RoleActions.Exists(x => x.ActionId == request.ActionId))
            {
                throw new BadRequestException("Vai trò không có chức năng này");
            }

            var action = await _actionReadRepository.GetByIdAsync(request.ActionId, cancellationToken: cancellationToken);
            if (action == null)
            {
                throw new BadRequestException("Chức năng không tồn tại");
            }

            await _roleWriteRepository.RemoveRoleActionAsync(role.RoleActions.First(x => x.ActionId == request.ActionId), cancellationToken);

            return Unit.Value;
        }
    }
}
