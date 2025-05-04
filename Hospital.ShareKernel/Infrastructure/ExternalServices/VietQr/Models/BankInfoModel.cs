namespace Hospital.SharedKernel.Infrastructure.ExternalServices.VietQr.Models
{
    public class BankInfoModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Code { get; set; }

        public string Bin { get; set; }

        public string Logo { get; set; }

        public int TransferSupported { get; set; }

        public int LookupSupported { get; set; }
    }
}
