using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay;
using Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.SharedKernel.Presentations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _VNPayService;

        public VNPayController(IVNPayService _vNPayService)
        {
            _VNPayService = _vNPayService;
        }

        [HttpPost("create-url")]
        public async Task<IActionResult> Create(VNpayModel model, CancellationToken cancellationToken = default)
        {
            var url = await _VNPayService.GenerateUrl(model, cancellationToken);
            return Ok(new SimpleDataResult { Data = url });
        }

        [HttpGet("payment-return")]
        public async Task<IActionResult> GetResponse(CancellationToken cancellationToken = default)
        {
            var result = await _VNPayService.PaymentExecute(Request.Query, cancellationToken);
            return Ok(new SimpleDataResult { Data = result });
        }

    }
}
