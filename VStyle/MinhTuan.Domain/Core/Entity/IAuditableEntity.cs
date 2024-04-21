using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Core.Entity
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        string? CreatedBy { get; set; }
        Guid? CreatedID { get; set; }

        DateTime UpdatedDate { get; set; }
        Guid? UpdatedID { get; set; }
        string? UpdatedBy { get; set; }
        bool? IsDelete { get; set; }

        DateTime? DeleteTime { get; set; }

        Guid? DeleteId { get; set; }
        string? DeleteBy { get; set; }
    }
}
