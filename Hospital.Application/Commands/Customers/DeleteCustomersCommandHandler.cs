using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Auth;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Customers
{
    public class DeleteCustomersCommandHandler : BaseCommandHandler, IRequestHandler<DeleteCustomersCommand>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        public DeleteCustomersCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            ICustomerWriteRepository customerWriteRepository,
            ICustomerReadRepository customerReadRepository,
            IAuthRepository authRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _authRepository = authRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
        }

        public async Task<Unit> Handle(DeleteCustomersCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || !request.Ids.Any())
            {
                return Unit.Value;
            }

            var customers = await _customerReadRepository.GetByIdsAsync(request.Ids, cancellationToken: cancellationToken);
            if (!customers.Any())
            {
                return Unit.Value;
            }

            await _authRepository.RemoveRefreshTokensAsync(customers.Select(x => x.Id), cancellationToken);

            await _customerWriteRepository.DeleteAsync(customers, cancellationToken);

            var forceLogoutTasks = customers.Select(x => _authService.ForceLogoutAsync(x.Id, cancellationToken));

            await Task.WhenAll(forceLogoutTasks);

            return Unit.Value;
        }
    }
}
