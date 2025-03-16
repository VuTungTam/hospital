using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Symptoms;
using Hospital.Domain.Entities.Bookings;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class AddSymptomsForBookingCommandHandler : BaseCommandHandler, IRequestHandler<AddSymptomsForBookingCommand>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly ISymptomReadRepository _symptomReadRepository;
        public AddSymptomsForBookingCommandHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IMapper mapper,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            ISymptomReadRepository symptomReadRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingWriteRepository = bookingWriteRepository;
            _bookingReadRepository = bookingReadRepository;
            _symptomReadRepository = symptomReadRepository;
        }

        public async Task<Unit> Handle(AddSymptomsForBookingCommand request, CancellationToken cancellationToken)
        {
            if (request.BookingId <= 0)
            {
                throw new BadRequestException("Phiếu khám không hợp lệ");
            }

            if (request.SymptomIds == null || !request.SymptomIds.Any() || request.SymptomIds.Any(aid => aid <= 0))
            {
                throw new BadRequestException("Triệu chứng không hợp lệ");
            }

            var option = new QueryOption
            {
                Includes = new string[] { nameof(Booking.BookingSymptoms) }
            };
            var booking = await _bookingReadRepository.GetByIdAsync(request.BookingId, option, cancellationToken);

            if (booking == null)
            {
                throw new BadRequestException("Phiếu khám không tồn tại");
            }

            var symptomIds = request.SymptomIds.Where(sid => !booking.BookingSymptoms.Any(bs => bs.SymptomId == sid)).ToList();
            if (!symptomIds.Any())
            {
                return Unit.Value;
            }

            var symptoms = await _symptomReadRepository.GetByIdsAsync(symptomIds, _symptomReadRepository.DefaultQueryOption, cancellationToken);
            if (symptoms.Count != symptoms.Count)
            {
                throw new BadRequestException("Bao gồm triệu chứng không tồn tại");
            }

            foreach (var symptomId in symptomIds)
            {
                booking.BookingSymptoms.Add(new BookingSymptom { SymptomId = symptomId, BookingId = request.BookingId });
            }

            await _bookingWriteRepository.UpdateAsync(booking, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
