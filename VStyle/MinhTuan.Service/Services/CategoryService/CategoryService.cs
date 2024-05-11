using AutoMapper;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Repository.CategoryRepositoy;
using MinhTuan.Service.Core;
using MinhTuan.Service.DTOs.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.CategoryService
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork,IMapper mapper, ICategoryRepository categoryRepository) : base(unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public CategoryDTO GetById(Guid id)
        {
            Category objectEntity = _categoryRepository.GetById(id);
           // var objectEntity = _categoryRepository.GetAll().FirstOrDefault();
            var objectDTO = _mapper.Map<CategoryDTO>(objectEntity);
            return objectDTO;
        }
    }
}
