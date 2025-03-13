using Hospital.SharedKernel.Application.Services.Auth.Entities;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Users
{
    [Table("Users")]
    public class User :
        BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        [Filterable("Mã")]
        public string Code { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        [Filterable("Số điện thoại")]
        public string Phone { get; set; }

        public bool PhoneVerified { get; set; }

        [Filterable("Email")]
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

        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public string Avatar { get; set; }

        public string ZaloId { get; set; }

        public string Provider { get; set; }

        public string PhotoUrl { get; set; }

        public string Json { get; set; }

        public int Shard { get; set; }

        public bool? IsCustomer { get; set; }

        public DateTime? LastPurchase { get; set; }

        public decimal TotalSpending { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }

        //[ForeignKey("OwnerId")]
        //public List<Avatar> Avatars { get; set; }
    }
}
