//using System.Linq.Expressions;

//namespace MinhTuan.Service.Core
//{
//    public interface IService<T> where T : class
//    {
//        T? GetById(Guid? id);

//        void Create(T entity);

//        void Update(T entity);

//        void Delete(T entity);

//        void SoftDelete(T entity);

//        IQueryable<T> GetQueryable();

//        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MinhTuan.Service.Core
{
    public interface IService<T> where T : class
    {
        T? GetById(Guid? id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        IQueryable<T> GetQueryable();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}
