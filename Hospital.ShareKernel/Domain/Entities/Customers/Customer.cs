using System.ComponentModel.DataAnnotations.Schema;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Domain.Entities.Customers
{
    [Table("mcs_customers")]
    public class Customer : BaseUser
    {
        public string Provider { get; set; }

        public bool EmailVerified { get; set; }
    }
}
