using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Employees
{
    public class ChangeRolesForEmployeeCommandHandler : BaseCommandHandler, IRequestHandler<ChangeRolesForEmployeeCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly ISocketService _socketService;

        public ChangeRolesForEmployeeCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IRoleReadRepository roleReadRepository,
            IEmployeeReadRepository employeeReadRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            ISocketService socketService
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _roleReadRepository = roleReadRepository;
            _employeeReadRepository = employeeReadRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _socketService = socketService;
        }

        public async Task<Unit> Handle(ChangeRolesForEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.PayloadIsNotValid"]);
            }

            if (request.RoleIds == null || !request.RoleIds.Any())
            {
                throw new BadRequestException("Phải có ít nhất 1 vai trò");
            }

            if (request.UserId == _executionContext.Identity)
            {
                throw new BadRequestException("Không thể tự cập nhật vai trò");
            }

            var user = await _employeeReadRepository.GetByIdIncludedRolesAsync(request.UserId, cancellationToken);

            _authService.ValidateStateAndThrow(user);

            if (user.EmployeeRoles?.FirstOrDefault(x => x.Role.Code == RoleCodeConstant.SUPER_ADMIN) != null)
            {
                throw new BadRequestException("Không được phép cập nhật thông tin quản trị hệ thống");
            }

            var roles = await _roleReadRepository.GetAsync(cancellationToken: cancellationToken);
            var roleIds = roles.Select(x => x.Id);
            if (request.RoleIds.Except(roleIds).Any())
            {
                throw new BadRequestException("Tồn tại vai trò đã bị xóa. Vui lòng kiểm tra lại");
            }
            if (request.RoleIds.Exists(id => id == roles.First(x => x.Code == RoleCodeConstant.SUPER_ADMIN).Id))
            {
                throw new BadRequestException("Không thể giao vai trò quản trị cho nhân viên khác. Để đảm bảo tính an toàn, vui lòng liên hệ nhà phát triển");
            }


            var oldRoleNames = string.Join(", ", roles.Where(x => user.EmployeeRoles.Select(u => u.Role.Id).Contains(x.Id)).Select(x => x.Name));
            var roleNames = string.Join(", ", roles.Where(x => request.RoleIds.Contains(x.Id)).Select(x => x.Name));

            await _employeeWriteRepository.UpdateRolesAsync(user.Id, request.RoleIds, cancellationToken);

            await _employeeWriteRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);

            //await _eventDispatcher.FireEventAsync(new UpdateRolesForEmployeeDomainEvent(user, oldRoleNames, roleNames), cancellationToken);

            await _authService.FetchNewTokenAsync(request.UserId, "Vai trò của bạn vừa được thay đổi. Tải lại?", cancellationToken);

            return Unit.Value;
        }
    }
}
