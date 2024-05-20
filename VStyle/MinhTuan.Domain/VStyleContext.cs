
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinhTuan.Domain.Core.Entity;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain
{
    
    public class VStyleContext : IdentityDbContext<AppUser>
    {
		private readonly IHttpContextAccessor _httpContextAccessor;

		public VStyleContext(DbContextOptions<VStyleContext> options,IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
			_httpContextAccessor = httpContextAccessor;

		}



		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var item in ChangeTracker.Entries())
			{
				if (item.Entity is IAuditableEntity entity)
				{
					var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
					var _user = this.Users.Where(x => x.UserName == userName).FirstOrDefault();
					switch (item.State)
					{
						case EntityState.Modified:
							entity.UpdatedDate = DateTime.Now;

							if (_user != null && _user.Id  != string.Empty)
							{
								entity.CreatedBy = _user.UserName;
								entity.CreatedID = Guid.Parse(_user.Id);
							}
							break;
						case EntityState.Added:
							entity.CreatedDate = DateTime.Now;
							if (_user != null && _user.Id != string.Empty)
							{
								entity.CreatedBy = _user.UserName;
								entity.CreatedID = Guid.Parse(_user.Id);
							}
							break;
						default:
							break;
					}
				}
			}
			return base.SaveChangesAsync(cancellationToken);
		}
		//Khai báo các thực thể cần sử dụng
		//ví dụ:
		// public DbSet<DanhMuc> DanhMucs { get; set; }
		//public DbSet<Category> categories { get; set; }
       
     

    }
    }
