using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Entities
{
    [Table("mcs_sequences")]
    public class Sequence :
        BaseEntity,

        IModifiedAt,
        IModifiedBy
    {
        public string Table { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public int Value { get; set; }

        public int Length { get; set; }

        public string ValueString => Prefix + GetOffset() + Value + Suffix;

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        private string GetOffset()
        {
            var valueLength = Value.ToString().Length;
            if (Length > valueLength)
            {
                return new string('0', Length - valueLength);
            }
            return string.Empty;
        }
    }
}
