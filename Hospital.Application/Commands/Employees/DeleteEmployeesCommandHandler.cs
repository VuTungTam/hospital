using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Employees;
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
    public class DeleteEmployeesCommandHandler : BaseCommandHandler, IRequestHandler<DeleteEmployeesCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;
        private readonly IAuthRepository _authRepository;

        public DeleteEmployeesCommandHandler(
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

        public async Task<Unit> Handle(DeleteEmployeesCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return Unit.Value;
            }
            if (request.Ids.Exists(x => x == _executionContext.Identity))
            {
                throw new BadRequestException("Không thể tự xóa tài khoản");
            }

            var superAdmins = await _employeeReadRepository.GetSuperAdminsAsync(cancellationToken);
            if (superAdmins.IntersectBy(request.Ids, x => x.Id).Any())
            {
                throw new ForbiddenException("Không được quyền xóa tài khoản quản trị");
            }

            var employees = await _employeeReadRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (!employees.Any())
            {
                return Unit.Value;
            }

            await _authRepository.RemoveRefreshTokensAsync(employees.Select(x => x.Id), cancellationToken);

            await _employeeWriteRepository.DeleteAsync(employees, cancellationToken);

            //await _eventDispatcher.FireEventAsync(new DeleteEmployeesDomainEvent(employees), cancellationToken);

            var forceLogoutTasks = employees.Select(x => _authService.ForceLogoutAsync(x.Id, cancellationToken));

            await Task.WhenAll(forceLogoutTasks);

            return Unit.Value;
        }
    }
}
