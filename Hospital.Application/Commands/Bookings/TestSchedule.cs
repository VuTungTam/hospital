using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Bookings
{
    public class TestSchedule : BaseAllowAnonymousCommand
    {
        public TestSchedule()
        {
        }
    }
}