using AutoMapper;
using HGO.ASPNetCore.FileManager.CommandsProcessor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhTuan.Application.ViewModels;
using MinhTuan.Service.Services.CategoryService;
using System.Diagnostics;

namespace MinhTuan.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IFileManagerCommandsProcessor _fileManager;

        public HomeController(ILogger<HomeController> logger, IMapper mapper,ICategoryService categoryService,
            IFileManagerCommandsProcessor fileManagerCommandsProcessor
            )
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
            _fileManager = fileManagerCommandsProcessor;
           
        }

        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult Privacy()
        {     
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404)
                {
                    var viewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = "Sorry, the resource you requested could not be found." };
                    return View(viewModel);
                }
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost, HttpGet]
        public async Task<IActionResult> HgoApi(string id , string command, string parameters, IFormFile file)
        {
            return await _fileManager.ProcessCommandAsync(id, command, parameters, file);
        }
    }
}
