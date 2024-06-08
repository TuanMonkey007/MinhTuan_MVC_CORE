using AutoMapper;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.DataCategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Domain.Repository.DataCategoryRepository;
using MinhTuan.Service.Core;

using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.DataCategoryService
{
    public class DataCategoryService : Service<DataCategory>, IDataCategoryService
    {
        private readonly IDataCategoryRepository _dataCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public DataCategoryService(IUnitOfWork unitOfWork, IMapper mapper,
            ICategoryRepository categoryRepository,
            IDataCategoryRepository dataCategoryRepository) : base(unitOfWork)
        {
            _mapper = mapper;
            _dataCategoryRepository = dataCategoryRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public bool CheckExitCode(string code, Guid parentId, Guid id)
        {
            if (id == Guid.Empty)
            {
                return _dataCategoryRepository.FindBy(x => x.Code == code && x.IsDelete != true &&x.ParentId == parentId).Any();
            }
            return _dataCategoryRepository.FindBy(x => x.Code == code && x.IsDelete != true && x.Id != id &&x.ParentId == parentId).Any();
        }

        public bool CheckExitName(string name, Guid parentId, Guid id)
        {
            if (id == Guid.Empty)
            {
                return _dataCategoryRepository.FindBy(x => x.Name == name && x.IsDelete != true && x.ParentId == parentId).Any();
            }
            return _dataCategoryRepository.FindBy(x => x.Name == name && x.IsDelete != true && x.Id != id && x.ParentId == parentId).Any();
        }

        public ResponseWithDataDto<List<DataCategoryDTO>> GetByParentCode(string parentCode)
        {
            var serverResponse = new ResponseWithDataDto<List<DataCategoryDTO>>() { Message = "Lấy danh mục con thành công" };

            try
            {
                Guid parentId = _categoryRepository.FindBy(x => x.Code.Contains(parentCode) && x.IsDelete != true).FirstOrDefault().Id;
                var dataCategories = _dataCategoryRepository.FindBy(x => x.ParentId.Equals(parentId) && x.IsDelete != true).ToList(); // Lấy danh sách danh mục con

                if (dataCategories == null || !dataCategories.Any()) // Kiểm tra xem có danh mục con nào không
                {
                    serverResponse.Message = "Không tìm thấy danh mục con";
                    return serverResponse;
                }

                // Sử dụng AutoMapper để chuyển đổi danh sách DataCategory thành DataCategoryDTO
                var dataCategoryDTOs = _mapper.Map<List<DataCategoryDTO>>(dataCategories);
                serverResponse.Data = dataCategoryDTOs;
                serverResponse.Status = "Success"; // Đặt trạng thái thành công

                return serverResponse;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                serverResponse.Status = "Fail";
                serverResponse.Message = "Lỗi khi lấy danh mục con: " + ex.Message;
                return serverResponse;
            }
        }

        public ResponseWithDataDto<List<DataCategoryDTO>> GetByParentId(Guid parentId)
        {
            var serverResponse = new ResponseWithDataDto<List<DataCategoryDTO>>() { Message = "Lấy danh mục con thành công" };

            try
            {
                var dataCategories = _dataCategoryRepository.FindBy(x => x.ParentId == parentId && x.IsDelete != true).ToList(); // Lấy danh sách danh mục con

                if (dataCategories == null || !dataCategories.Any()) // Kiểm tra xem có danh mục con nào không
                {
                    serverResponse.Message = "Không tìm thấy danh mục con";
                    return serverResponse;
                }

                // Sử dụng AutoMapper để chuyển đổi danh sách DataCategory thành DataCategoryDTO
                var dataCategoryDTOs = _mapper.Map<List<DataCategoryDTO>>(dataCategories);
                serverResponse.Data = dataCategoryDTOs;
                serverResponse.Status = "Success"; // Đặt trạng thái thành công

                return serverResponse;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                serverResponse.Status = "Fail";
                serverResponse.Message = "Lỗi khi lấy danh mục con: " + ex.Message;
                return serverResponse;
            }
        }


        public ResponseWithDataDto<PagedList<DataCategoryDTO>> GetDataByPage(DataCategorySearchDTO searchDTO)
        {
            try
            {
                var query = from entityTbl in _dataCategoryRepository.GetQueryable()
                            where entityTbl.IsDelete != true //Xét xóa mềm
                            select new DataCategoryDTO
                            {
                                Id = entityTbl.Id,
                                ParentId = entityTbl.ParentId,
                                Name = entityTbl.Name ?? string.Empty,
                                Code = entityTbl.Code ?? string.Empty,
                                Description = entityTbl.Description ?? string.Empty
                            };


                if (searchDTO != null)
                {
                    if(searchDTO.ParentId_Filter.HasValue)
                    {
                      
                        var list = _dataCategoryRepository.GetQueryable().Select(x => x.ParentId).ToList().Where(x => x.Equals(searchDTO.ParentId_Filter));
                        query = query.Where(x => list.Contains(x.ParentId));
                    }
                    if (!string.IsNullOrEmpty(searchDTO.Name_Filter))
                    {
                        var idSearch = searchDTO.Name_Filter.ToString();
                        var isNormal = searchDTO.Name_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _dataCategoryRepository.GetQueryable().Select(x => x.Name).ToList().Where(x => x.ToString().ToLower().RemoveAccentsUnicode().Contains(idSearch.ToLower().RemoveAccentsUnicode()));
                        query = query.Where(x => list.Contains(x.Name));
                    }
                    if (!string.IsNullOrEmpty(searchDTO.Code_Filter))
                    {
                        var idSearch = searchDTO.Code_Filter.ToString();
                        var isNormal = searchDTO.Code_Filter.ToString().ToLower() != idSearch.ToLower();
                        var list = _dataCategoryRepository.GetQueryable().Select(x => x.Code).ToList().Where(x => x.ToString().ToLower().Contains(idSearch.ToLower()));
                        query = query.Where(x => list.Contains(x.Code));
                    }
                }
                var result = PagedList<DataCategoryDTO>.Create(query, searchDTO);
                return new ResponseWithDataDto<PagedList<DataCategoryDTO>>()
                {
                    Data = result,

                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<DataCategoryDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }

        public Guid GetIdByCode(string code)
        {
            return _dataCategoryRepository.FindBy(x=>x.Code.Equals(code) && x.IsDelete != true).FirstOrDefault().Id;
        }

        public string GetNameById(Guid id)
        {
           return  _dataCategoryRepository.GetById(id).Name;
        }
        public string GetCodeById(Guid id)
        {
            return _dataCategoryRepository.GetById(id).Code;
        }

    }
}
