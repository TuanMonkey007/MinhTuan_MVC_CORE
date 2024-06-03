using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("Banners")]
    public class Banner:AuditableEntity
    {
        public string? Path { get; set; }
      
        public Guid CategoryId { get; set; }
        public int OrderDisplay { get; set; }
        public bool IsDisplay { get; set; }
       
    }
}
