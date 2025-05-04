using Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr.Models;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr
{
    public interface IVietQrService
    {
        Task<List<BankInfoModel>> GetBanksAsync(CancellationToken cancellationToken = default);

        Task<string> GenerateQrAsync(BankQrModel info, CancellationToken cancellationToken = default);
    }
}
