using AutoMapper;
using MinhTuan.API.ViewModels;
using MinhTuan.API.ViewModels.AccountViewModel;
using MinhTuan.API.ViewModels.CategoryViewModel;
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
        CreateMap<UserDTO, AppUser>().ReverseMap();
        CreateMap<ForgotPasswordViewModel, ForgotPasswordDTO>().ReverseMap();
        CreateMap<ChangePasswordDTO, ChangePasswordViewModel>().ReverseMap();
        CreateMap<CategoryDTO, CreateCategoryViewModel>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();
    }
}