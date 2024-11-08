using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Newses
{
    [Table("News")]
    public class News
      : BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        public string Image { get; set; }

        public string Slug { get; set; }

        public string TitleSeo { get; set; }

        [Filterable("Tiêu đề")]
        public string Title { get; set; }

        public string TitleEn { get; set; }

        public string Content { get; set; }

        public string ContentEn { get; set; }

        public string Toc { get; set; }

        public string TocEn { get; set; }

        public DateTime? PostDate { get; set; }

        public NewsStatus Status { get; set; }

        public bool? IsHighlight { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public long? Creator { get; set; }

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
