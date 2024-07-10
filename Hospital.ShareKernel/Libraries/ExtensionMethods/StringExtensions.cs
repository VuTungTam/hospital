namespace Hospital.SharedKernel.Libraries.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToSnakeCaseLowers(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString())).ToLower();
        }
        public static string FirstCharToUpper(this string input) => input switch
        {
            null => null,
            "" => "",
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
    }
}
