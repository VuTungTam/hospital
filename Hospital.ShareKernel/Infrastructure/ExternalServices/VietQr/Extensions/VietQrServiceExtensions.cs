using Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr.Extensions
{
    public static class VietQrServiceExtensions
    {
        public static IServiceCollection AddVietQrService(this IServiceCollection services)
        {
            return services.AddScoped<IVietQrService, VietQrService>();
        }
    }
}
