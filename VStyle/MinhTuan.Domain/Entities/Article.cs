using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("Articles")]
    public class Article:AuditableEntity
    {

        public string Title { get; set; }
        public string Summary { get; set; }
        public string  Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string Slug { get; set; }
        public string[] Tag { get; set; }
        public int CountView   { get; set; }
        public bool Status { get; set; }
        public string Thumbnail { get; set; }
       
    }
}
