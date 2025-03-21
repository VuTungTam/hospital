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
    public class UpdateLastSeenCommandHandler : BaseCommandHandler, IRequestHandler<UpdateLastSeenCommand>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IServiceProvider _serviceProvider;

        public UpdateLastSeenCommandHandler(
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

        public async Task<Unit> Handle(UpdateLastSeenCommand request, CancellationToken cancellationToken)
        {
            if (_executionContext.AccountType == AccountType.Employee)
            {
                await _serviceProvider.GetRequiredService<IEmployeeWriteRepository>().UpdateLastSeenAsync(cancellationToken);
            }
            else if (_executionContext.AccountType == AccountType.Customer)
            {
                await _serviceProvider.GetRequiredService<ICustomerWriteRepository>().UpdateLastSeenAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
