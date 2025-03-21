using AutoMapper;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Customers
{
    public class GetCustomerNamesQueryHandler : BaseQueryHandler, IRequestHandler<GetCustomerNamesQuery, List<CustomerNameDto>>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomerNamesQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ICustomerReadRepository customerReadRepository
        ) : base(authService, mapper, localizer)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<List<CustomerNameDto>> Handle(GetCustomerNamesQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerReadRepository.GetCustomerNamesAsync(cancellationToken);
            return _mapper.Map<List<CustomerNameDto>>(customers);
        }
    }
}
