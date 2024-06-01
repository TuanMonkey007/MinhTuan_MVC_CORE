using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Core.Repository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    IQueryable<T> GetQueryable();
    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
    T Add(T entity);
    T Delete(T entity);
    void Update(T entity);
    Task Save();
    T? GetById(object id);
    Task<T?> GetByIdAsync(object id);
    void DeleteRange(IEnumerable<T> entities);
    void SoftDeleteRange(IEnumerable<T> entities);
    void AddRange(IEnumerable<T> entities);
    void UpdateRange(IEnumerable<T> entities);
    void SoftDelete(T entity);
}
