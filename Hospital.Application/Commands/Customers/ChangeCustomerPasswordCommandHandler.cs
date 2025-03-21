using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Libraries.Security;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class ChangeCustomerPasswordCommandHandler : BaseCommandHandler, IRequestHandler<ChangeCustomerPasswordCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public ChangeCustomerPasswordCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<Unit> Handle(ChangeCustomerPasswordCommand request, CancellationToken cancellationToken)
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

            var customer = await _customerReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

            _authService.ValidateStateAndThrow(customer);

            if (!PasswordHasher.Verify(request.Dto.OldPassword, customer.PasswordHash))
            {
                throw new BadRequestException(_localizer["Account.OldPasswordIsIncorrect"]);
            }

            customer.Password = request.Dto.NewPassword;
            customer.IsDefaultPassword = false;
            customer.IsPasswordChangeRequired = false;
            customer.HashPassword();

            await _customerWriteRepository.UpdateAsync(customer, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
