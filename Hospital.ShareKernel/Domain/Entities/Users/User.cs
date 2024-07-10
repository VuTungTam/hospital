using Hospital.SharedKernel.Application.Service.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.SharedKernel.Domain.Entities.Users
{
    public class User :
        BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete
    {
        [Filterable("Mã")]
        public string Code { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        [Filterable("Số điện thoại")]
        public string Phone { get; set; }

        public bool PhoneVerified { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        [Filterable("Tên")]
        public string Name { get; set; }

        public DateTime? Dob { get; set; }

        public int Pid { get; set; }

        public string Pname { get; set; }

        public int Did { get; set; }

        public string Dname { get; set; }

        public int Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public string Avatar { get; set; }

        public string ZaloId { get; set; }

        public string Provider { get; set; }

        public string PhotoUrl { get; set; }

        public string Json { get; set; }

        public bool? IsCustomer { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
