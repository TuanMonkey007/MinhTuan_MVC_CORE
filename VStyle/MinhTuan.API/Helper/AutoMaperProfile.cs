using AutoMapper;
using MinhTuan.API.ViewModels;
using MinhTuan.API.ViewModels.AccountViewModel;
using MinhTuan.API.ViewModels.CategoryViewModel;
using MinhTuan.API.ViewModels.DataCategoryViewModel;
using MinhTuan.Domain.DTOs.AccountDTO;
using MinhTuan.Domain.DTOs.CategoryDTO;
using MinhTuan.Domain.DTOs.DataCategoryDTO;
using MinhTuan.Domain.Entities;

namespace MinhTuan.Application.Helper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //Tạo mapper tài khoản
        CreateMap<AppUser, LogInDTO>().ReverseMap();
        CreateMap<LogInDTO, LogInViewModel>().ReverseMap();
        CreateMap<AppUser, RegisterDTO>().ReverseMap();
        CreateMap<RegisterDTO, RegisterViewModel>().ReverseMap();
        CreateMap<RegisterDTO, CreateAccountViewModel>().ReverseMap();
        CreateMap<UserDTO, AppUser>().ReverseMap();
        CreateMap<UserDTO, CreateAccountViewModel>().ReverseMap();
        CreateMap<ForgotPasswordViewModel, ForgotPasswordDTO>().ReverseMap();
        CreateMap<ChangePasswordDTO, ChangePasswordViewModel>().ReverseMap();

        //Tạo mapper danh mục
        CreateMap<CategoryDTO, CreateCategoryViewModel>().ReverseMap();
        CreateMap<Category, CategoryDTO>().ReverseMap();

        //Tạo maper dữ liệu danh mục
        CreateMap<DataCategory, DataCategoryDTO>().ReverseMap();
        CreateMap<DataCategoryDTO, CreateDataCategoryViewModel>().ReverseMap();

    }
}