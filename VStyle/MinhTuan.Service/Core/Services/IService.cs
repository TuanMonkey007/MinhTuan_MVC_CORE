
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MinhTuan.Service.Core.Services
{
    public interface IService<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid? id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SoftDeleteAsync(T entity);
        IQueryable<T> GetQueryable(); // Không cần đổi vì không phải async
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
    }

}
