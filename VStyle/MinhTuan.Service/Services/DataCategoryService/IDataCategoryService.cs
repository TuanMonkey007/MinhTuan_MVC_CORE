﻿using MinhTuan.Domain.Core.DTO;
using MinhTuan.Domain.DTOs.DataCategoryDTO;
using MinhTuan.Domain.Entities;
using MinhTuan.Domain.Helper.Pagination;
using MinhTuan.Service.Core.Services;

using MinhTuan.Service.SearchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Service.Services.DataCategoryService
{
    public interface IDataCategoryService : IService<DataCategory>
    {
        ResponseWithDataDto<PagedList<DataCategoryDTO>> GetDataByPage(DataCategorySearchDTO searchDTO);
        bool CheckExitCode(string code,Guid parentId, Guid id);
        bool CheckExitName(string name,Guid parentId, Guid id);
    }
}