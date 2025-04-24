using System.Text.RegularExpressions;

namespace Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Maps.Utils
{
    public class MapUtility
    {
        public static bool IsMapURL(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var pattern = @"^(https?:\/\/)?(www\.)?(google\.[a-z]{2,3}(\.[a-z]{2})?\/maps|goo\.gl\/maps)";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
    }
}
