using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr;
using Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.SharedKernel.Presentations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VietQrController : ControllerBase
    {
        private readonly IVietQrService _vietQrService;

        public VietQrController(IVietQrService vietQrService)
        {
            _vietQrService = vietQrService;
        }

        [HttpGet("banks")]
        public async Task<IActionResult> GetBanks(CancellationToken cancellationToken = default)
        {
            var banks = await _vietQrService.GetBanksAsync(cancellationToken);
            return Ok(new ServiceResult { Data = banks, Total = banks.Count });
        }

        [HttpPost("generate-qr")]
        public async Task<IActionResult> GenerateQr(BankQrModel info, CancellationToken cancellationToken = default)
        {
            var url = await _vietQrService.GenerateQrAsync(info, cancellationToken);
            return Ok(new SimpleDataResult { Data = url });
        }
    }
}
