using AutoMapper;
using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Application.Repositories.Interfaces.Customers;
using Hospital.Application.Services.Interfaces.Sockets;
using Hospital.Domain.Entities.Bookings;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Models;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using Hospital.SharedKernel.Application.Services.Date;
using Hospital.SharedKernel.Domain.Events.Interfaces;
using Hospital.SharedKernel.Infrastructure.Databases.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Libraries.Utils;
using Hospital.SharedKernel.Modules.Notifications.Entities;
using Hospital.SharedKernel.Modules.Notifications.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Commands.Bookings
{
    public class TestScheduleHandler : BaseCommandHandler, IRequestHandler<TestSchedule>
    {
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingWriteRepository _bookingWriteRepository;
        private readonly IRedisCache _redisCache;
        private readonly IDateService _dateService;
        private readonly ISocketService _socketService;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        public TestScheduleHandler(
            IEventDispatcher eventDispatcher,
            IAuthService authService,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache,
            IBookingReadRepository bookingReadRepository,
            IBookingWriteRepository bookingWriteRepository,
            ISocketService socketService,
            IMapper mapper,
            IDateService dateService,
            ICustomerWriteRepository customerWriteRepository
            ) : base(eventDispatcher, authService, localizer, mapper)
        {
            _bookingReadRepository = bookingReadRepository;
            _bookingWriteRepository = bookingWriteRepository;
            _redisCache = redisCache;
            _dateService = dateService;
            _socketService = socketService;
            _customerWriteRepository = customerWriteRepository;
        }

        public Task<Unit> Handle(TestSchedule request, CancellationToken cancellationToken)
        {
            _bookingWriteRepository.TestSchedule();
            return Task.FromResult(Unit.Value);
        }
    }
}
