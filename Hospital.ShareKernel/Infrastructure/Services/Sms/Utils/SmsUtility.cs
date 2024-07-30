using System.Text.RegularExpressions;

namespace Hospital.SharedKernel.Infrastructure.Services.Sms.Utils
{
    public class SmsUtility
    {
        public static string CleanPhoneNumber(string input)
        {
            return Regex.Replace(input, "[^0-9]", "");
        }

        public static bool IsVietnamesePhone(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(input, "^(84|\\+84|0)[35789]\\d{8}$");
        }
    }
}
