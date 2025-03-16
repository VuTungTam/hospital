using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Auths
{
    [Table("mcs_login_histories")]
    public class LoginHistory :
        BaseEntity,
        ICreatedAt
    {
        public long UserId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Ip { get; set; }

        //public string Browser { get; set; }

        //public string OS { get; set; }

        //public string Device { get; set; }

        public string UA { get; set; }

        //public string City { get; set; }

        //public string Country { get; set; }

        //public string Lat { get; set; }

        //public string Long { get; set; }

        //public string Timezone { get; set; }

        //public string Org { get; set; }

        //public string Postal { get; set; }

        public string Origin { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
