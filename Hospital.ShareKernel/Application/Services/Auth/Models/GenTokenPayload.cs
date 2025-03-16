using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Application.Services.Auth.Models
{
    public class GenTokenPayload
    {
        public BaseUser User { get; set; }

        public string Permission { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public int ValidSeconds { get; set; } = AuthConfig.TokenTime;
    }
}
