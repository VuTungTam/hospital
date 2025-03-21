using AutoMapper;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Repositories.Interfaces.Auth.Actions;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.ExecutionContext;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Customers
{
    public class GetCustomerProfileQueryHandler : BaseQueryHandler, IRequestHandler<GetCustomerProfileQuery, CustomerDto>
    {
        private readonly IActionReadRepository _actionReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IExecutionContext _executionContext;

        public GetCustomerProfileQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            IActionReadRepository actionReadRepository,
            ICustomerReadRepository customerReadRepository,
            IExecutionContext executionContext
        ) : base(authService, mapper, localizer)
        {
            _actionReadRepository = actionReadRepository;
            _customerReadRepository = customerReadRepository;
            _executionContext = executionContext;
        }

        public async Task<CustomerDto> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerReadRepository.GetByIdAsync(_executionContext.Identity, cancellationToken: cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
