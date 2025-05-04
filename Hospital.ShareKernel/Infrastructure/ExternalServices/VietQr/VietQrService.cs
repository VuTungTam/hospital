using Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr.Models;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Runtime.Exceptions;
using Newtonsoft.Json;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr
{
    public class VietQrService : IVietQrService
    {
        private readonly IHttpClientFactory _factory;

        public VietQrService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public Task<string> GenerateQrAsync(BankQrModel info, CancellationToken cancellationToken = default)
        {
            if (info == null)
            {
                throw new BadRequestException("Thông tin tạo mã QR không hợp lệ");
            }

            if (info.BankId <= 0)
            {
                throw new BadRequestException("BIN không hợp lệ");
            }

            if (string.IsNullOrEmpty(info.AccountNo))
            {
                throw new BadRequestException("Tài khoản nhận tiền không hợp lệ");
            }

            info.AccountName = info.AccountName?.ViToEn() ?? "";
            info.AddInfo = info.AddInfo?.ViToEn() ?? "";

            var url = $"https://api.vietqr.io/image/{info.BankId}-{info.AccountNo}-W2Fzq1W.jpg?accountName={info.AccountName}&amount={info.Amount}&addInfo={info.AddInfo}";
            return Task.FromResult(url);
        }

        public async Task<List<BankInfoModel>> GetBanksAsync(CancellationToken cancellationToken = default)
        {
            using (var client = _factory.CreateClient())
            {
                var url = "https://api.vietqr.io/v2/banks";
                var response = await client.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new CatchableException("Xảy ra lỗi trong lúc lấy danh sách ngân hàng");
                }

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<VietQrResult>(content);
                var banks = JsonConvert.DeserializeObject<List<BankInfoModel>>(JsonConvert.SerializeObject(result.Data));

                return banks;
            }
        }
    }
}
