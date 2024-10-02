namespace Hospital.SharedKernel.Infrastructure.Services.Websites.Utils
{
    public class WebsiteUtility
    {
        public static bool BeAValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _);
        }
    }
}
