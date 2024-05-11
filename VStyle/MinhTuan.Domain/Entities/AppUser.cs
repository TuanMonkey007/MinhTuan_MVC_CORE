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
    public string FullName { get; set; }
}
