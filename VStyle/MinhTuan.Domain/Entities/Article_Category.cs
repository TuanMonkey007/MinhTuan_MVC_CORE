using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("ArticleCategories")]
    public class Article_Category:AuditableEntity
    {
        public Guid ArticleId { get; set; }
        public Guid CategoryId { get; set; }
        // Navigation properties
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
        [ForeignKey("CategoryId")]
        public virtual DataCategory Category { get; set; }
    }
}
