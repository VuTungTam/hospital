using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Articles
{
    [Table("tbl_articles")]
    public class Article
      : BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
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

        public string Summary { get; set; }

        public string SummaryEn { get; set; }

        public string Toc { get; set; }

        public string TocEn { get; set; }

        public DateTime? PostDate { get; set; }

        public ArticleStatus Status { get; set; }

        public bool? IsHighlight { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
