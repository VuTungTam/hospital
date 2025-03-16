using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Application.Services.Auth.Models;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using IdGen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using UAParser;
namespace Hospital.SharedKernel.Libraries.Utils
{
    public static class AuthUtility
    {
        private static object LockObj = new object();
        private static IdGenerator Generator = new IdGenerator(0);

        /// <summary>
        /// Tạo id theo twitter's snowflake
        /// </summary>
        public static long GenerateSnowflakeId()
        {
            lock (LockObj)
            {
                return Generator.CreateId();
            }
        }

        /// <summary>
        /// Kiểm tra endpoint có cần authorize không?
        /// </summary>
        public static bool EndpointRequiresAuthorize(ResourceExecutingContext context)
        {
            var endpointMetadata = context.ActionDescriptor.EndpointMetadata;
            var allowAnonymous = endpointMetadata.FirstOrDefault(x => x.GetType() == typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute));
            if (allowAnonymous != null)
            {
                return false;
            }

            var authorize = endpointMetadata.FirstOrDefault(x => x.GetType() == typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));
            if (authorize == null)
            {
                return false;
            }
            return true;
        }

        public static string FromExponentToPermission(int exponent)
        {
            var result = new BigInteger(1);
            var two = new BigInteger(2);

            for (int i = 1; i <= exponent; i++)
                result *= two;

            return result.ToString();
        }

        public static string CalculateToTalPermision(IEnumerable<int> exponents)
        {
            var result = new BigInteger(0);
            foreach (var exponent in exponents)
                result += BigInteger.Parse(FromExponentToPermission(exponent));
            return result.ToString();
        }

        public static string ConvertToBinary(string input)
        {
            var result = "";
            var parse = BigInteger.Parse(input);
            var two = new BigInteger(2);

            while (true)
            {
                var b = parse % two;
                parse = parse / two;
                result += b;
                if (parse.IsZero)
                {
                    break;
                }
            }
            return string.Join("", result.Reverse());
        }

        public static bool ComparePermissionAsString(string permission, string acttionPermission)
        {
            if (string.IsNullOrEmpty(permission) || string.IsNullOrEmpty(acttionPermission))
            {
                return false;
            }
            var left = ConvertToBinary(permission);
            var right = ConvertToBinary(acttionPermission);
            var offset = "";
            var andResult = "";

            // Ensure Left always greater than Right
            if (right.Length > left.Length)
            {
                var tmp = left;
                left = right;
                right = tmp;
            }

            // Compensate for the number on the right
            for (int i = 1; i <= left.Length - right.Length; i++)
            {
                offset += "0";
            }
            right = offset + right;

            // Execute bitwise &
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] == right[i] && left[i] == '1')
                    andResult += "1";
                else
                    andResult += "0";
            }

            return andResult.Equals(right);
        }

        public static string TryGetIP(HttpRequest request)
        {
            var ip = request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }
            var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;
            if (remoteIpAddress != null)
            {
                // If we got an IPV6 address, then we need to ask the network for the IPV4 address
                // This usually only happens when the browser is on the same machine as the server.
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                    .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                ip = remoteIpAddress.ToString();
            }
            return ip;
        }

        public static bool IsValidIpAddress(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Count(c => c == '.') != 3)
                return false;
            return IPAddress.TryParse(input, out var _);
        }

        public static ValidPasswordResult ValidatePassword(string pwd, int minLength, PasswordLevel requiresLevel = PasswordLevel.Strong)
        {
            var result = new ValidPasswordResult(requiresLevel);
            if (string.IsNullOrEmpty(pwd))
            {
                result.ValidLength = false;
                return result;
            }

            var satisfiedCount = 0;
            if (pwd.Length >= minLength)
            {
                satisfiedCount++;
                result.ValidLength = true;
            }

            if (Regex.IsMatch(pwd, "[A-Z]"))
            {
                satisfiedCount++;
                result.HasUpperCase = true;
            }

            if (Regex.IsMatch(pwd, "[a-z]"))
            {
                satisfiedCount++;
                result.HasLowerCase = true;
            }

            if (Regex.IsMatch(pwd, "[0-9]"))
            {
                satisfiedCount++;
                result.HasNumber = true;
            }

            if (Regex.IsMatch(pwd, "[!@#$%^&*(),.?\":{}|<>]"))
            {
                satisfiedCount++;
                result.HasSpecialChar = true;
            }

            switch (requiresLevel)
            {
                case PasswordLevel.Weak:
                    result.IsValid = result.ValidLength;
                    break;
                case PasswordLevel.Medium:
                    result.IsValid = result.ValidLength && satisfiedCount >= 3;
                    break;
                case PasswordLevel.Strong:
                    result.IsValid = result.ValidLength && satisfiedCount == 5;
                    break;
            }

            return result;
        }

        public static BasicRequestInfo GetBasicRequestInformation(HttpRequest request)
        {
            var ua = request.Headers[HeaderNames.UserAgent].ToString();
            return new BasicRequestInfo
            {
                Ip = TryGetIP(request),
                ApiUrl = $"{request.Scheme}://{request.Host}{request.Path.Value}{request.QueryString.Value}",
                Method = request.Method,
                Origin = request.Headers[HeaderNames.Origin],
                UA = ua
            };
        }

        public static RequestInfo GetRequestInfo(HttpRequest request)
        {
            var ua = request.Headers[HeaderNames.UserAgent].ToString();
            var c = Parser.GetDefault().Parse(ua);

            return new RequestInfo
            {
                RequestId = Guid.NewGuid().ToString(),
                Ip = TryGetIP(request),
                ApiUrl = $"{request.Scheme}://{request.Host}{request.Path.Value}{request.QueryString.Value}",
                Method = request.Method,
                Origin = request.Headers[HeaderNames.Origin],
                Device = c.Device.Family,
                Browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : ""),
                OS = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") + (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : ""),
            };
        }

        public static async Task<IpInformation> GetIpInformationAsync(IServiceProvider provider, string ip)
        {
            var redisCache = provider.GetRequiredService<IRedisCache>();
            var cacheEntry = CacheManager.GetClientInformationCacheEntry(ip);
            var data = await redisCache.GetAsync<IpInformation>(cacheEntry.Key);
            if (data != null)
            {
                return data;
            }

            var client = HttpClientFactory.Create();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var token = configuration.GetRequiredSection("External:IpInfoToken").Value;
            var result = await client.GetAsync($"https://ipinfo.io/{ip}?token={token}");

            if (result.IsSuccessStatusCode)
            {
                var info = JsonConvert.DeserializeObject<IpInformation>(await result.Content.ReadAsStringAsync());
                await redisCache.SetAsync(cacheEntry.Key, info, TimeSpan.FromDays(cacheEntry.ExpiriesInSeconds));
                return info;
            }
            return null;
        }
    }


    public class ValidPasswordResult
    {
        public ValidPasswordResult(PasswordLevel requiresLevel)
        {
            RequiresLevel = requiresLevel;
        }

        public bool IsValid { get; set; }

        public bool ValidLength { get; set; }

        public bool HasLowerCase { get; set; }

        public bool HasUpperCase { get; set; }

        public bool HasNumber { get; set; }

        public bool HasSpecialChar { get; set; }

        public PasswordLevel RequiresLevel { get; }
    }

    public enum PasswordLevel
    {
        [Description("Yếu")]
        Weak = 1,

        [Description("Trung bình")]
        Medium = 3,

        [Description("Mạnh")]
        Strong = 5
    }
}
