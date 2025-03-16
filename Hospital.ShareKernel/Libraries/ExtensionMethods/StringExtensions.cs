using System.Text.RegularExpressions;
using System.Text;

namespace Hospital.SharedKernel.Libraries.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
            => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        public static string ToSnakeCaseLower(this string input)
            => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString())).ToLower();

        public static string ToKebabCase(this string input)
            => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString()));

        public static string ToKebabCaseLower(this string input)
            => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString().ToLower() : x.ToString())).ToLower();

        public static string ToMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string ToBase64Encode(this string plainText)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        public static string ToBase64Decode(this string base64EncodedData)
        {
            if (base64EncodedData == null)
            {
                throw new ArgumentNullException("");
            }
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string FirstCharToUpper(this string input) => input switch
        {
            null => null,
            "" => "",
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };

        public static string ReplaceRegex(this string value, string pattern, string replacement)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return string.Empty;
                }

                value = value.Trim();
                return Regex.Replace(value, pattern, replacement);
            }
            catch
            {
                return value;
            }
        }

        public static string NormalizeString(this string value)
        {
            try
            {
                return value.ReplaceRegex("\\s+", " ");
            }
            catch
            {
                return value;
            }
        }

        public static string ViToEn(this string unicodeString, bool special = false)
        {
            if (string.IsNullOrEmpty(unicodeString))
            {
                return string.Empty;
            }

            try
            {
                unicodeString = unicodeString.NormalizeString();
                unicodeString = unicodeString.Trim();
                unicodeString = Regex.Replace(unicodeString, "[áàảãạâấầẩẫậăắằẳẵặ]", "a");
                unicodeString = Regex.Replace(unicodeString, "[éèẻẽẹêếềểễệ]", "e");
                unicodeString = Regex.Replace(unicodeString, "[iíìỉĩị]", "i");
                unicodeString = Regex.Replace(unicodeString, "[óòỏõọơớờởỡợôốồổỗộ]", "o");
                unicodeString = Regex.Replace(unicodeString, "[úùủũụưứừửữự]", "u");
                unicodeString = Regex.Replace(unicodeString, "[yýỳỷỹỵ]", "y");
                unicodeString = Regex.Replace(unicodeString, "[đ]", "d");
                if (special)
                {
                    unicodeString = Regex.Replace(unicodeString, "[\"`~!@#$%^&*()-+=?/>.<,{}[]|]\\]", "");
                }

                unicodeString = unicodeString.Replace("\u0300", "").Replace("\u0323", "").Replace("\u0309", "").Replace("\u0303", "").Replace("\u0301", "");
                return unicodeString;
            }
            catch
            {
                return "";
            }
        }

        public static bool HasUnicode(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }
            var length = source.Length;

            source = Regex.Replace(source, "[áàảãạâấầẩẫậăắằẳẵặ]", "");
            source = Regex.Replace(source, "[éèẻẽẹêếềểễệ]", "");
            source = Regex.Replace(source, "[iíìỉĩị]", "");
            source = Regex.Replace(source, "[óòỏõọơớờởỡợôốồổỗộ]", "");
            source = Regex.Replace(source, "[úùủũụưứừửữự]", "");
            source = Regex.Replace(source, "[yýỳỷỹỵ]", "");
            source = Regex.Replace(source, "[đ]", "");

            return source.Length != length;
        }

        public static bool IsNumber(this char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;

                default:
                    return false;
            }
        }

        public static string StripHtml(this string input,string value = "")
        {
            return Regex.Replace(input, "<.*?>", value);
        }

        public static string GetUpperChars(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var str = string.Empty;
            foreach (var c in input)
            {
                if (c >= 65 && c <= 90)
                {
                    str += c;
                }
            }
            return str;
        }
    }
}
