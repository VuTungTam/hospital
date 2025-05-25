using AutoMapper;
using Hospital.Application.Dtos.Payments;
using Hospital.Application.Repositories.Interfaces.Payments;
using Hospital.Domain.Specifications.Payments;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Payments
{
    public class GetPaymentByTransactionIdQueryHandler : BaseQueryHandler, IRequestHandler<GetPaymentByTransactionIdQuery, PaymentDto>
    {
        private readonly IPaymentReadRepository _paymentReadRepository;
        public GetPaymentByTransactionIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IPaymentReadRepository paymentReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _paymentReadRepository = paymentReadRepository;
        }

        public async Task<PaymentDto> Handle(GetPaymentByTransactionIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TransactionId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var payment = await _paymentReadRepository.GetByTransactionId(request.TransactionId, cancellationToken);

            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
