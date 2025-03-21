using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
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
    public class UpdateCustomerStatusCommandHandler : BaseCommandHandler, IRequestHandler<UpdateCustomerStatusCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IAuthRepository _authRepository;

        public UpdateCustomerStatusCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IAuthRepository authRepository
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _authRepository = authRepository;
        }

        public async Task<Unit> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException("Id không hợp lệ");
            }

            if (request.Status == AccountStatus.None)
            {
                return Unit.Value;
            }

            var customer = await _customerReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            if (customer == null)
            {
                throw new BadRequestException("Khách hàng không tồn tại");
            }

            if (customer.Status == request.Status)
            {
                return Unit.Value;
            }

            var oldStatus = customer.Status;

            customer.Status = request.Status;
            if (customer.Status != AccountStatus.Active)
            {
                await _authRepository.RemoveRefreshTokensAsync(new List<long> { customer.Id }, cancellationToken);
            }

            //customer.AddDomainEvent(new UpdateCustomerStatusDomainEvent(customer, oldStatus));

            await _customerWriteRepository.UpdateAsync(customer, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
