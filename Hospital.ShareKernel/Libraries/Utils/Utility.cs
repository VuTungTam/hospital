using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Hospital.SharedKernel.Libraries.ExtensionMethods;

namespace Hospital.SharedKernel.Libraries.Utils
{
    public static class Utility
    {
        public static string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        }

        public static bool IsPrimitiveType(object obj)
        {
            if (obj == null)
                return false;

            switch (obj.GetType().Name)
            {
                case "Boolean":
                case "Byte":
                case "SByte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Char":
                case "Double":
                case "Single":
                    return true;

                default:
                    return false;
            }
        }

        public static string RandomNumber(int length)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            while (sb.Length < length)
            {
                int num = random.Next(0, 9);
                sb.Append(num + "");
            }

            return sb.ToString();
        }

        public static string RandomString(int length, bool isUpper = false, bool hasNumber = true, int increaseNumberTo = 1)
        {
            var random = new Random();
            var mix = !isUpper ? Enumerable.Range(97, 26) : Enumerable.Range(65, 26).Concat(Enumerable.Range(97, 26));
            if (hasNumber)
            {
                var numbers = Enumerable.Range(48, 10);
                for (int i = 0; i < increaseNumberTo; i++)
                {
                    numbers = numbers.Concat(Enumerable.Range(48, 10));
                }

                mix = mix.Concat(numbers);
            }

            var result = new List<char>();
            var mixCount = mix.Count();
            if (length <= mixCount)
            {
                var str = string.Join("", mix.OrderBy(x => random.Next()).Take(length).Select(x => (char)x));
                return isUpper ? str.ToUpper() : str;
            }

            while (length > 0)
            {
                result.AddRange(mix.OrderBy(x => random.Next()).Take(length).Select(x => (char)x));
                length -= mixCount;
            }

            return isUpper ? string.Join("", result).ToUpper() : string.Join("", result);
        }

        public static string GetContentType(string subpath)
        {
            var success = new FileExtensionContentTypeProvider().TryGetContentType(subpath, out var contentType);
            if (success)
            {
                return contentType;
            }
            return "other";
        }

        public static string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ViToEn().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 100 ? str.Length : 100).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
