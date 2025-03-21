using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Repositories.Interfaces.Employees;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Accounts
{
    public class UpdateAvatarCommandHandler : BaseCommandHandler, IRequestHandler<UpdateAvatarCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IServiceProvider _serviceProvider;

        public UpdateAvatarCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IExecutionContext executionContext,
            IServiceProvider serviceProvider
        ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _executionContext = executionContext;
            _serviceProvider = serviceProvider;
        }

        public async Task<Unit> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            if (_executionContext.AccountType == AccountType.Employee)
            {
                var readrp = _serviceProvider.GetRequiredService<IEmployeeReadRepository>();
                var employee = await readrp.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

                _authService.ValidateStateAndThrow(employee);

                employee.Avatar = request.FileName;

                var writerp = _serviceProvider.GetRequiredService<IEmployeeWriteRepository>();
                await writerp.UpdateAsync(employee, cancellationToken: cancellationToken);
            }
            else if (_executionContext.AccountType == AccountType.Customer)
            {

                var readrp = _serviceProvider.GetRequiredService<ICustomerReadRepository>();
                var customer = await readrp.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

                _authService.ValidateStateAndThrow(customer);

                customer.Avatar = request.FileName;

                var writerp = _serviceProvider.GetRequiredService<ICustomerWriteRepository>();
                await writerp.UpdateAsync(customer, cancellationToken: cancellationToken);
            }

            return Unit.Value;
        }
    }
}
