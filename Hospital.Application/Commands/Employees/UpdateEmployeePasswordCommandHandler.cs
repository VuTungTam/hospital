using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Employees
{
    public class UpdateEmployeePasswordCommandHandler : BaseCommandHandler, IRequestHandler<UpdateEmployeePasswordCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;

        public UpdateEmployeePasswordCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IEmployeeReadRepository employeeReadRepository,
            IEmployeeWriteRepository employeeWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _employeeReadRepository = employeeReadRepository;
            _employeeWriteRepository = employeeWriteRepository;
        }

        public async Task<Unit> Handle(UpdateEmployeePasswordCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeReadRepository.GetByIdIncludedRolesAsync(request.Model.UserId, cancellationToken);
            if (employee == null)
            {
                throw new BadRequestException("Tài khoản không tồn tại");
            }

            if (employee.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException("Tài khoản đã bị khóa");
            }

            var roles = employee.EmployeeRoles.Select(x => x.Role).ToList();
            var isSuperAdmin = roles.Any(x => x.Code == RoleCodeConstant.SUPER_ADMIN);
            if (isSuperAdmin && _executionContext.Identity != employee.Id)
            {
                throw new ForbiddenException("Không được quyền cập nhật tài khoản quản trị");
            }

            await _authService.CheckPasswordLevelAndThrowAsync(request.Model.NewPassword, cancellationToken);

            employee.Password = request.Model.NewPassword;
            employee.IsDefaultPassword = false;
            employee.IsPasswordChangeRequired = false;
            employee.HashPassword();
            //employee.AddDomainEvent(new UpdateEmployeePasswordDomainEvent(employee));

            await _employeeWriteRepository.UpdateAsync(employee, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
