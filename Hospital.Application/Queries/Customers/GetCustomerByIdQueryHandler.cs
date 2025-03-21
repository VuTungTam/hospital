using AutoMapper;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Customers
{
    public class GetCustomerByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomerByIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ICustomerReadRepository customerReadRepository
        ) : base(authService, mapper, localizer)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var customer = await _customerReadRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
