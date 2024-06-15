using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.DTOs.BannerDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Domain.Repository.BannerRepository;
using MinhTuan.Domain.Repository.DataCategoryRepository;
using MinhTuan.Service.Core;
using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace MinhTuan.Service.Services.BannerService
{
    public class BannerService : Service<Banner>, IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly IDataCategoryRepository _dataCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BannerService(IUnitOfWork unitOfWork,IBannerRepository bannerRepository, IMapper mapper,IDataCategoryRepository dataCategoryRepository) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bannerRepository = bannerRepository;
            _mapper = mapper;
            _dataCategoryRepository = dataCategoryRepository;
        }
        private static string ImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Banner", imagePath);
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                return Convert.ToBase64String(imageBytes);
            }
            catch
            {
                return null; // Hoặc trả về một giá trị mặc định khác
            }
        }
        private static string GetContentTypeFromFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Banner", fileName)).ToLowerInvariant();
            return fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
        }


        public  ResponseWithDataDto<PagedList<BannerDTO>> GetDataByPage(BannerSearchDTO searchDTO)
        {
            try
            {
                 var query = from banner in _bannerRepository.GetQueryable()
                            join category in _dataCategoryRepository.GetQueryable() // Nối bảng BannerCategory
                                on banner.CategoryId equals category.Id
                            where banner.IsDelete != true
                            select new BannerDTO
                            {
                                Id = banner.Id,
                                Path = banner.Path ?? string.Empty,
                                CategoryId = banner.CategoryId,
                                CategoryName = category.Name, // Thêm CategoryName vào DTO
                                OrderDisplay = banner.OrderDisplay,
                                IsDisplay = banner.IsDisplay
                            };
                if (!searchDTO.CategoryCode_Filter.IsNullOrEmpty())
                {
                    searchDTO.CategoryId_Filter = _dataCategoryRepository.FindBy(x => x.Code.Equals(searchDTO.CategoryCode_Filter)).FirstOrDefault().Id;
                    if (!searchDTO.CategoryCode_Filter.IsNullOrEmpty())
                    {
                        query = query.Where(x => x.CategoryId.Equals(searchDTO.CategoryId_Filter));
                    }

                }
                if (searchDTO.IsDisplay_Filter ==true){
                    query = query.Where(x => x.IsDisplay.Equals(searchDTO.IsDisplay_Filter));
                }
                if (!string.IsNullOrEmpty(searchDTO.sortQuery))
                {
                    query = query.OrderBy(searchDTO.sortQuery);
                }
                else
                {
                    query = query.OrderBy(x => x.CategoryName);
                }

                var result = PagedList<BannerDTO>.Create(query, searchDTO);
                // Gán ảnh thumbnail vào ProductDTO
                foreach (var bannerDTO in result.Items)
                {
                    bannerDTO.BannerBase64 = ImageToBase64(bannerDTO.Path);
                    bannerDTO.BannerContentType = GetContentTypeFromFileName(bannerDTO.Path);
                 }
                return new ResponseWithDataDto<PagedList<BannerDTO>>()
                {
                    Data = result,
                    Message = "Lấy thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseWithDataDto<PagedList<BannerDTO>>()
                {
                    Data = null,

                    Message = ex.Message

                };
            }
        }

    }
}
