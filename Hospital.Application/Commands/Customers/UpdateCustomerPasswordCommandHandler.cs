using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class UpdateCustomerPasswordCommandHandler : BaseCommandHandler, IRequestHandler<UpdateCustomerPasswordCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public UpdateCustomerPasswordCommandHandler(
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

        public async Task<Unit> Handle(UpdateCustomerPasswordCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerReadRepository.GetByIdAsync(request.Model.UserId, cancellationToken: cancellationToken);
            if (customer == null)
            {
                throw new BadRequestException("Tài khoản không tồn tại");
            }

            if (customer.Status == AccountStatus.Blocked)
            {
                throw new BadRequestException("Tài khoản đã bị khóa");
            }

            await _authService.CheckPasswordLevelAndThrowAsync(request.Model.NewPassword, cancellationToken);

            customer.Password = request.Model.NewPassword;
            customer.IsDefaultPassword = false;
            customer.IsPasswordChangeRequired = false;
            customer.HashPassword();
            await _customerWriteRepository.UpdateAsync(customer, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
