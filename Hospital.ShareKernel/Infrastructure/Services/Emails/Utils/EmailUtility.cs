using System.Net.Mail;

namespace Hospital.SharedKernel.Infrastructure.Services.Emails.Utils
{
    public class EmailUtility
    {
        public static bool IsEmail(string input)
        {
            try
            {
                new MailAddress(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
