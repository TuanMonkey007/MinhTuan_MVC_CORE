using MinhTuan.Domain.Entities;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.DTOs.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.CategoryService
{
    public interface ICategoryService:IService<Category>
    {
        CategoryDTO GetById(Guid id);
        List<CategoryDTO> GetAllCategory();
    }
}
