namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models
{
    public class VNPayIPN
    {
        public string VNPTxnRef { get; set; }

        public string VNPTransactionNo { get; set; }

        public string VNPResponseCode { get; set; }

        public string VNPSecureHash { get; set; }
    }
}
