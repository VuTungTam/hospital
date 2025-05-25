namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VNPay.Models
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public int Amount { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
        public DateTime Date { get; set; }
    }
}
