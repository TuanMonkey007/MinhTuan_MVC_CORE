using Microsoft.AspNetCore.Identity;
using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Entities;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public Guid? Gender { get; set; }
    public DateOnly? BirthDay { get; set; }
    public string? AccountType { get; set; }
    public string?  Avatar { get; set; }
    public string?  Address { get; set; }
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }

}
