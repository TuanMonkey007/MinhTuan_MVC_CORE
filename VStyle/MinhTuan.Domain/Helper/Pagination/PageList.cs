using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Helper.Pagination
{
    public class PagedList<T>
    {
        public PagedList()
        {
        }

        private PagedList(List<T> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

            public static PagedList<T> Create(IQueryable<T> query, SearchBase search)
            {
                var totalCount = query.Count();
                if (search.PageSize != -1)
                {
                    var items = query.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList();
                    return new PagedList<T>(items, search.PageIndex, search.PageSize, totalCount);
                }
                else
                {
                    var items = query.ToList();
                    return new PagedList<T>(items, search.PageIndex, search.PageSize, totalCount);
                }
            }


    }
}
