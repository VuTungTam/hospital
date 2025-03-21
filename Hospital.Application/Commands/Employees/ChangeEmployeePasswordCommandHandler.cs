using AutoMapper;
using Hospital.Application.Commands.Employees;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Employees
{
    public class ChangeEmployeePasswordCommandHandler : BaseCommandHandler, IRequestHandler<ChangeEmployeePasswordCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IEmployeeReadRepository _employeeReadRepository;
        private readonly IEmployeeWriteRepository _employeeWriteRepository;

        public ChangeEmployeePasswordCommandHandler(
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

        public async Task<Unit> Handle(ChangeEmployeePasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Dto.OldPassword))
            {
                throw new BadRequestException(_localizer["Account.OldPasswordMustNotBeEmpty"]);
            }

            if (string.IsNullOrEmpty(request.Dto.NewPassword))
            {
                throw new BadRequestException(_localizer["Account.NewPasswordMustNotBeEmpty"]);
            }

            await _authService.CheckPasswordLevelAndThrowAsync(request.Dto.NewPassword, cancellationToken);

            var employee = await _employeeReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

            _authService.ValidateStateAndThrow(employee);

            if (!PasswordHasher.Verify(request.Dto.OldPassword, employee.PasswordHash))
            {
                throw new BadRequestException(_localizer["Account.OldPasswordIsIncorrect"]);
            }

            employee.Password = request.Dto.NewPassword;
            employee.IsDefaultPassword = false;
            employee.IsPasswordChangeRequired = false;
            employee.HashPassword();

            await _employeeWriteRepository.UpdateAsync(employee, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
