using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.Application.Areas.Category.Models;
using MinhTuan.Application.Controllers;
using MinhTuan.Service.Services.CategoryService;
using MinhTuan.Domain.Entities;
using MinhTuan.Service.DTOs.CategoryDTO;
namespace MinhTuan.Application.Areas.Category.Controllers;

[Area("Category")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
   

    public CategoryController(ICategoryService categoryService, IMapper mapper
       )
    {
        _categoryService = categoryService;
        _mapper = mapper;
       
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult GetAllCategory()
    {
        var result = _categoryService.GetAllCategory();
        return Json(result);
    }
    public PartialViewResult ShowModal_Create()
    {
        var model = new CreateViewModel();
        return PartialView("_CreatePartial", model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public JsonResult Create(CreateViewModel model)
    {
        var result = "Tạo danh mục thành công";
        if (ModelState.IsValid)
        {
            //Check trùng
            var EntityDTO =_mapper.Map<CategoryDTO>(model);
            var EntityModel = _mapper.Map<MinhTuan.Domain.Entities.Category>(EntityDTO);
            _categoryService.Create(EntityModel);
        }
        else
        {
            result = "Dữ liệu không hợp lệ";
        }
      
        return Json(result);
    }
}