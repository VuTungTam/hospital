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
    public class GetPaymentValidByBookingIdQueryHandler : BaseQueryHandler, IRequestHandler<GetPaymentValidByBookingIdQuery, PaymentDto>
    {
        private readonly IPaymentReadRepository _paymentReadRepository;
        public GetPaymentValidByBookingIdQueryHandler(
            IAuthService authService,
            IMapper mapper,
            IPaymentReadRepository paymentReadRepository,
            IStringLocalizer<Resources> localizer
            ) : base(authService, mapper, localizer)
        {
            _paymentReadRepository = paymentReadRepository;
        }

        public async Task<PaymentDto> Handle(GetPaymentValidByBookingIdQuery request, CancellationToken cancellationToken)
        {
            if (request.BookingId <= 0)
            {
                throw new BadRequestException(_localizer["CommonMessage.IdIsNotValid"]);
            }

            var spec = new GetPaymentsByBookingIdSpecification(request.BookingId);

            var option = new QueryOption
            {
                IgnoreFacility = true
            };
            //Sua lai cho nay
            var payments = await _paymentReadRepository.GetAsync(option, cancellationToken);

            var payment = payments.FirstOrDefault();

            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
