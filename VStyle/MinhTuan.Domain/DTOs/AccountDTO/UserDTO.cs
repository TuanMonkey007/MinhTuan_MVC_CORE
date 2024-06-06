using Microsoft.AspNetCore.Http;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.DTOs.AccountDTO;

public class UserDTO
{
    public string Id { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public string?  PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public Guid? Gender { get; set; }
    public string? NameGender { get; set; }
    public DateOnly? BirthDay { get; set; }
    public string? AccountType { get; set; }
    public string? Avatar { get; set; }
    public string? Address { get; set; }
    public bool? EmailConfirmed { get; set; }
  //  public IFormFile AvatarImage { get; set; }

    public string? AvatarBase64 { get; set; } // Chuỗi base64 của ảnh

    public string? AvatarContentType { get; set; } // Loại nội dung của ảnh (ví dụ: image/jpeg)
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
}
