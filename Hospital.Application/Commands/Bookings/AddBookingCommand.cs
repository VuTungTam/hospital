﻿using Hospital.Application.Dtos.Bookings;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    [RequiredPermission(ActionExponent.BookingManagement)]
    public class AddBookingCommand : BaseAllowAnonymousCommand<string>
    {
        public AddBookingCommand(BookingDto booking)
        {
            Booking = booking;
        }

        public BookingDto Booking { get; }
    }
}
