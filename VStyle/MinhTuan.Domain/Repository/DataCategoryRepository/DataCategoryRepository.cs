using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.DataCategoryRepository
{
    public class DataCategoryRepository : Repository<DataCategory>, IDataCategoryRepository
    {
        public DataCategoryRepository(VStyleContext context) : base(context)
        {
        }
    }
}
