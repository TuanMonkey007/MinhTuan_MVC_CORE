using AutoMapper;
using MinhTuan.Application.ViewModels;
using MinhTuan.Application.ViewModels.AccountViewModel;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.DTOs.CategoryDTO;
namespace MinhTuan.Application.Helper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, LogInDTO>().ReverseMap();
        CreateMap<LogInDTO, LogInViewModel>().ReverseMap();
        CreateMap<AppUser, RegisterDTO>().ReverseMap();
        CreateMap<RegisterDTO, RegisterViewModel>().ReverseMap();
        // Thêm các ánh xạ khác tại đây nếu cần
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<CategoryDTO, CategoryViewModel>().ReverseMap();
        CreateMap<CategoryDTO, Category>();
        CreateMap<Category, CategoryDTO>();
    }
}