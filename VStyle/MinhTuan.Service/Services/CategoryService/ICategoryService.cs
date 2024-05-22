using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;
using MinhTuan.Service.DTOs.CategoryDTO;
using MinhTuan.Service.SearchDTO;
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
        ResponseWithDataDto<PagedList<CategoryDTO>> GetDataByPage(CategorySearchDTO searchDTO);
    
        bool CheckExitCode(string code, Guid id );
        bool CheckExitName(string name, Guid id );
    }
}
