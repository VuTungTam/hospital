using Hospital.SharedKernel.Infrastructure.Services.Sms.Utils;
using System.Text.RegularExpressions;

namespace Hospital.Application.Helpers
{
    public static class PhoneHelper
    {
        public static string TransferToDomainPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return "";
            }

            if (phone.StartsWith("+84"))
            {
                var regex = new Regex(Regex.Escape("+84"));
                phone = regex.Replace(phone, "0", 1);
            }

            if (phone.StartsWith("84"))
            {
                var regex = new Regex(Regex.Escape("84"));
                phone = regex.Replace(phone, "0", 1);
            }

            return SmsUtility.CleanPhoneNumber(phone);
        }
    }
}
