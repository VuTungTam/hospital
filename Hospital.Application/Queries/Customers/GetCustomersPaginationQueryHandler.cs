using AutoMapper;
using Hospital.Application.Dtos.Customers;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Customers
{
    public class GetCustomersPaginationQueryHandler : BaseQueryHandler, IRequestHandler<GetCustomersPaginationQuery, PaginationResult<CustomerDto>>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomersPaginationQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IStringLocalizer<Resources> localizer,
            ICustomerReadRepository customerReadRepository
        ) : base(authService, mapper, localizer)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<PaginationResult<CustomerDto>> Handle(GetCustomersPaginationQuery request, CancellationToken cancellationToken)
        {
            var pagination = await _customerReadRepository.GetCustomersPaginationResultAsync(request.Pagination, request.State, cancellationToken);
            var dtos = _mapper.Map<List<CustomerDto>>(pagination.Data);

            return new PaginationResult<CustomerDto>(dtos, pagination.Total);
        }
    }
}
