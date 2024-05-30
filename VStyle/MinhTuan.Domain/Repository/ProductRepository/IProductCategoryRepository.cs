using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.ProductRepository
{
    public interface IProductCategoryRepository: IRepository<Product_Category>
    {
    }
}
