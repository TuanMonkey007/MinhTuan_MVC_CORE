using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Dashboard.ProductService
{
    public interface IProductService:IService<Product>
    {
        public string getAll();
    }
}
