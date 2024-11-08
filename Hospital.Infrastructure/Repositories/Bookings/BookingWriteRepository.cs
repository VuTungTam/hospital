using Hospital.Application.Repositories.Interfaces.Bookings;
using Hospital.Domain.Entities.Bookings;
using Hospital.Infra.Repositories;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Hospital.Infrastructure.Repositories.Bookings
{
    public class BookingWriteRepository : WriteRepository<Booking>, IBookingWriteRepository
    {
        public BookingWriteRepository(
            IServiceProvider serviceProvider,
            IStringLocalizer<Resources> localizer,
            IRedisCache redisCache
        ) : base(serviceProvider, localizer, redisCache)
        {
        }

        public async Task AddWithCodeAsync(Booking booking, CancellationToken cancellationToken)
        {
            var table = "booking";
            var sequenceRepository = _serviceProvider.GetRequiredService<ISequenceRepository>();
            var code = await sequenceRepository.GetSequenceAsync(table, cancellationToken);

            booking.Code = code.ValueString;

            await sequenceRepository.IncreaseValueAsync(table, cancellationToken);
        }

    }
}
