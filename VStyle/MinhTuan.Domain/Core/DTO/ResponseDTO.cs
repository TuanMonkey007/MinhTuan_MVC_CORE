using Microsoft.AspNetCore.Http;
using MinhTuan.Domain.Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Core.DTO;
public class ResponseWithMessageDto
{
    public string Message { get; set; }
    public string Status { get; set; } = ApiStatus.SUCCESS;
    public int Code { get; set; } = StatusCodes.Status200OK;
}

public class ResponseWithDataDto<T>
{
   
    public string? Status { get; set; } = ApiStatus.SUCCESS;
    public T? Data { get; set; }
    public string? Message { get; set; }
    public int Code { get; set; } = StatusCodes.Status200OK;
}