using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities
{
    public class Product:AuditableEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
