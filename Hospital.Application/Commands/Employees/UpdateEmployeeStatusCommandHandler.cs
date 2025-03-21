using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Employees;
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
    public class UpdateEmployeeAccountStatusCommandHandler : BaseCommandHandler, IRequestHandler<UpdateEmployeeStatusCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IAuthRepository _authRepository;

        public UpdateEmployeeAccountStatusCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IEmployeeReadRepository employeeReadRepository,
            IEmployeeWriteRepository employeeWriteRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _employeeReadRepository = employeeReadRepository;
            _employeeWriteRepository = employeeWriteRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(UpdateEmployeeStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException("Id không hợp lệ");
            }

            if (request.Status == AccountStatus.None)
            {
                return Unit.Value;
            }

            if (request.Id == _executionContext.Identity)
            {
                throw new BadRequestException("Không thể tự cập nhật trạng thái");
            }

            var superAdmins = await _employeeReadRepository.GetSuperAdminsAsync(cancellationToken);
            if (superAdmins.Exists(x => x.Id == request.Id))
            {
                throw new ForbiddenException("Không được quyền cập nhật tài khoản quản trị");
            }

            var employee = await _employeeReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (employee == null)
            {
                throw new BadRequestException("Nhân viên không tồn tại");
            }

            if (employee.Status == request.Status)
            {
                return Unit.Value;
            }

            var oldStatus = employee.Status;

            employee.Status = request.Status;
            if (employee.Status != AccountStatus.Active)
            {
                await _authRepository.RemoveRefreshTokensAsync(new List<long> { employee.Id }, cancellationToken);
            }

            //employee.AddDomainEvent(new UpdateEmployeeStatusDomainEvent(employee, oldStatus));

            await _employeeWriteRepository.UpdateAsync(employee, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
