using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    [Table("DataCategories")]
    public class DataCategory:AuditableEntity
    {
        public Guid ParentId { get; set; }
        public string Code { get; set; }
       
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
