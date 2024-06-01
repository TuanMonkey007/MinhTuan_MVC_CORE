using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.ProductRepository
{
    public class ProductVariantRepository : Repository<Product_Variant>, IProductVariantRepository
    {
        public ProductVariantRepository(VStyleContext context) : base(context)
        {
        }
    }
}
