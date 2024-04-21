using Autofac.Core;
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Dashboard.ProductService
{
    public class ProductService : Service<Product>, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public string getAll()
        {
            var product = _unitOfWork.ProductRepo.GetById(Guid.Parse("a3042b23-a912-48c9-47f9-08dc5133c54f"));
            return product?.Name ?? "Product not found";

        }
    }
}
