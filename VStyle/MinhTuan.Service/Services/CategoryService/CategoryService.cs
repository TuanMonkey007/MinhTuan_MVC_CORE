using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Service.Core;
using MinhTuan.Service.DTOs.CategoryDTO;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MinhTuan.Service.Services.CategoryService
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public bool CheckExitCode(string code, Guid id)
        {
            if(id == Guid.Empty)
            {
                return _categoryRepository.FindBy(x => x.Code == code && x.IsDelete != true).Any();
            }
            return _categoryRepository.FindBy(x => x.Code == code && x.IsDelete != true && x.Id != id).Any();

        }

        public bool CheckExitName(string name, Guid id)
        {
            if(id == Guid.Empty)
            {
                return _categoryRepository.FindBy(x => x.Name == name && x.IsDelete != true).Any();
            }
            return _categoryRepository.FindBy(x => x.Name == name && x.IsDelete != true && x.Id != id).Any();
        }


        public List<CategoryDTO> GetAllCategory()
        {
            List<Category> result = _categoryRepository.GetAll().ToList();
            return _mapper.Map<List<CategoryDTO>>(result);
        }

        public CategoryDTO GetById(Guid id)
        {
            Category objectEntity = _categoryRepository.GetById(id);
            var objectDTO = _mapper.Map<CategoryDTO>(objectEntity);
            return objectDTO;
        }

        public ResponseWithDataDto<PagedList<CategoryDTO>> GetDataByPage(CategorySearchDTO searchDTO)
        {
            try
            {
                var query = from entityTbl in _categoryRepository.GetQueryable()
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new CategoryDTO
                            {
                                Id = entityTbl.Id,
                                Name = entityTbl.Name ?? string.Empty,
                                Code = entityTbl.Code ?? string.Empty,
                                Description = entityTbl.Description ?? string.Empty
                            };


                if (searchDTO != null)
                {
                    if (!string.IsNullOrEmpty(searchDTO.Name_Filter))
                    {
                        var idSearch = searchDTO.Name_Filter.ToString();
                        var isNormal = searchDTO.Name_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _categoryRepository.GetQueryable().Select(x => x.Name).ToList().Where(x => x.ToString().ToLower().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Name));
                    }
                    if (!string.IsNullOrEmpty(searchDTO.Code_Filter))
                    {
                        var idSearch = searchDTO.Code_Filter.ToString();
                        var isNormal = searchDTO.Code_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _categoryRepository.GetQueryable().Select(x => x.Code).ToList().Where(x => x.ToString().ToLower().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Code));
                    }
                }
                var result = PagedList<CategoryDTO>.Create(query, searchDTO);
                return new ResponseWithDataDto<PagedList<CategoryDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<CategoryDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }
    }
}
