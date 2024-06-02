using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.BannerRepository
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        public BannerRepository(VStyleContext context) : base(context)
        {
        }
    }
}
