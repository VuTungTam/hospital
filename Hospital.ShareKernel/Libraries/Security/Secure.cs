using System.Text.RegularExpressions;

namespace Hospital.SharedKernel.Libraries.Security
{
    public static class Secure
    {
        public static string MsgDetectedSqlInjection = "Sql injection detected. Please re-check your parameters.";

        public static bool DetectSqlInjection(string input)
        {
            //Regex reg = new Regex(@"\s?or\s*|\s?;\s?|\s?drop\s|\s?grant\s|^'|\s?--|/s?union\s|\s?delete\s|\s?truncate\s|\s?sysobjects\s?|\s?xp_.*?|\s?syslogins\s?|/s?sysremote\s?|\s?sysusers\s?|\s?sysxlogins\s?|\s?sysdatabases\s?|\s?aspnet_.*?|\s?exec\s?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex reg = new Regex(@"('(''|[^'])*')|(;)|(\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return !string.IsNullOrWhiteSpace(input) && reg.IsMatch(input);
        }

    }
}
