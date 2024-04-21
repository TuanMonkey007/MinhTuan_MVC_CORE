using System.ComponentModel.DataAnnotations;

namespace MinhTuan.Domain.Core.Entity
{
    public abstract class AuditableEntity : Entity, IAuditableEntity
    {
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string? CreatedBy { get; set; }

        public Guid? CreatedID { get; set; }

        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; }

        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string? UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        public Guid? UpdatedID { get; set; }

        public bool? IsDelete { get; set; }

        public DateTime? DeleteTime { get; set; }

        public Guid? DeleteId { get; set; }
        public string? DeleteBy { get; set; }

    }
}
