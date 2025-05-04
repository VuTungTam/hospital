namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr.Models
{
    public class BankQrModel
    {
        public int BankId { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public long Amount { get; set; }

        public string AddInfo { get; set; }

        public string Format { get; set; } = "text";

        public string Template { get; set; } = "print";
    }
}
