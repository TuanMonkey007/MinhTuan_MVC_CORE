using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.DataCategoryDTO
{
    public class DataCategoryDTO
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  DateTime? CreatedDate { get; set; }
    }
}
