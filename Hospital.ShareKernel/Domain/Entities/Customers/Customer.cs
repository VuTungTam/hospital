using Hospital.SharedKernel.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Customers
{
    [Table("mcs_customers")]
    public class Customer : BaseUser
    {
        public bool PhoneVerified { get; set; }

        public bool EmailVerified { get; set; }

        public DateTime? LastPurchase { get; set; }

        public decimal TotalSpending { get; set; }
    }
}
