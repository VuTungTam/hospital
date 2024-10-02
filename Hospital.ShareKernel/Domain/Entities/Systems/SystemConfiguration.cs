using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Systems
{
    [Table("mcs_system_configurations")]
    public class SystemConfiguration
      : BaseEntity,

        IModified,
        IModifier
    {
        public PasswordLevel RequiresPasswordLevel { get; set; }

        public bool? IsEnabledVerifiedAccount { get; set; }

        public int? Session { get; set; }

        public int? PasswordMinLength { get; set; }

        public int MaxNumberOfSmsPerDay { get; set; }

        public bool? PreventCopying { get; set; }



        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }
    }
}
