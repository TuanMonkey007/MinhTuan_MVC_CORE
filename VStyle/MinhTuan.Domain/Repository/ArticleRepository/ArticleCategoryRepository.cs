using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Repository.ArticleRepository
{
    public class ArticleCategoryRepository : Repository<Article_Category>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(VStyleContext context) : base(context)
        {
        }
    }
}
