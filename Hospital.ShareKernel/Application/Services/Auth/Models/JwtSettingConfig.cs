using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.SharedKernel.Application.Services.Auth.Models
{
    public class JwtSettingsConfig
    {
        public static string SecretKey { get; private set; }
        public static string Issuer { get; private set; }
        public static string Audience { get; private set; }

        public static void Set(IConfiguration configuration)
        {
            SecretKey = configuration.GetRequiredSection("Auth:JwtSettings:SecretKey").Value;
            Issuer = configuration.GetRequiredSection("Auth:JwtSettings:Issuer").Value;
            Audience = configuration.GetRequiredSection("Auth:JwtSettings:Audience").Value;
        }
    }
}
