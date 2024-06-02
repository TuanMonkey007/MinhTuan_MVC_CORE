
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
            // Tạo một bản sao của ChangeTracker.Entries() để tránh sửa đổi tập hợp gốc trong khi duyệt
            var entries = ChangeTracker.Entries().ToList();
            foreach (var item in entries)
			{
				if (item.Entity is IAuditableEntity entity)
				{
					var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                    var _user = this.Users.Where(x => x.FullName == userName).FirstOrDefault();
                 
               
                    switch (item.State)
					{
						case EntityState.Modified:
							

							if (_user != null)
							{
                                if (entity.IsDelete == true)
                                {
                                    // Nếu là soft delete, cập nhật thông tin xóa
                                    entity.DeleteBy = _user.FullName;
                                    entity.DeleteId = Guid.Parse(_user.Id);
                                    entity.DeleteTime = DateTime.Now;
                                }
                                else
                                {
                                    // Nếu không phải soft delete, cập nhật thông tin sửa đổi
                                    entity.UpdatedDate = DateTime.Now;
                                    entity.UpdatedBy = _user.FullName;
                                    entity.UpdatedID = Guid.Parse(_user.Id);
                                }
                            }
							break;
						case EntityState.Added:
							entity.CreatedDate = DateTime.Now;
							if (_user != null)
							{
								entity.CreatedBy = _user.FullName;
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
		public DbSet<Category> categories { get; set; }
		public DbSet<DataCategory> dataCategories { get; set; }
		public DbSet<Product> Products { get; set; }
        public DbSet<Product_Variant> Product_Variants { get; set; }

		public DbSet<Product_Image> Product_Images { get; set; }
		public DbSet<Product_Category> Product_Categories { get; set; }

		public DbSet<Banner> banners { get; set; }

	}
}
