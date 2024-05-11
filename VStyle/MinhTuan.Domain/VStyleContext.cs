
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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


        public VStyleContext(DbContextOptions<VStyleContext> options)
            : base(options)
        {

        }

       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        //Khai báo các thực thể cần sử dụng
        //ví dụ:
        // public DbSet<DanhMuc> DanhMucs { get; set; }
           public DbSet<Category> categories { get; set; }
       
     

    }
    }
