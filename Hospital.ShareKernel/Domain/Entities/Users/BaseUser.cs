using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.SharedKernel.Libraries.Security;

namespace Hospital.SharedKernel.Domain.Entities.Users
{
    public class BaseUser :
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
        ISoftDelete,
        IDeletedBy
    {
        [Filterable("Mã")]
        [Immutable]
        public string Code { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        [Filterable("Số điện thoại")]
        public string Phone { get; set; }

        [Filterable("Email")]
        public string Email { get; set; }

        [Filterable("Tên")]
        public string Name { get; set; }

        public DateTime? Dob { get; set; }

        public Gender Gender { get; set; }

        public int Pid { get; set; }

        public string Pname { get; set; }

        public int Did { get; set; }

        public string Dname { get; set; }

        public int Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public string Avatar { get; set; }

        public bool IsDefaultPassword { get; set; }

        public bool IsPasswordChangeRequired { get; set; }

        public DateTime? LastSeen { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }

        public void HashPassword()
        {
            PasswordHash = PasswordHasher.Hash(Password ?? "");
        }
    }
}
