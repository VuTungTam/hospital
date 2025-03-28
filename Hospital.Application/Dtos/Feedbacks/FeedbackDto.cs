﻿using Hospital.Domain.Entities.Bookings;

namespace Hospital.Application.Dtos.Feedbacks
{
    public class FeedbackDto : BaseDto
    {

        public int Stars { get; set; }

        public string Message { get; set; }

        public long BookingId { get; set; }

        public long? CreatedBy { get; set; }

        public long? ModifiedBy { get; set; }

        public long OwnerId { get; set; }

        public string BookingCode { get; set; }
    }
}
