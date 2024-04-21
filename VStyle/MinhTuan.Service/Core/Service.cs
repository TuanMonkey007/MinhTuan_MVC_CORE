//using MinhTuan.Domain.Core.UnitOfWork;
//using System.Linq.Expressions;

//namespace MinhTuan.Service.Core
//{
//    public class Service<T> : IService<T> where T : class
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public Service(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public void Create(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void Delete(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        public T? GetById(Guid? id)
//        {
//            throw new NotImplementedException();
//        }

//        public IQueryable<T> GetQueryable()
//        {
//            throw new NotImplementedException();
//        }

//        public void SoftDelete(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void Update(T entity)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MinhTuan.Domain.Core.UnitOfWork;

namespace MinhTuan.Service.Core
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(T entity)
        {
            _unitOfWork.GetRepository<T>().Add(entity);
            _unitOfWork.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _unitOfWork.GetRepository<T>().Delete(entity);
            _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.GetRepository<T>().FindBy(predicate);
        }

        public T? GetById(Guid? id)
        {
            return _unitOfWork.GetRepository<T>().GetById(id);
        }

        public IQueryable<T> GetQueryable()
        {
            return _unitOfWork.GetRepository<T>().GetQueryable();
        }

        public void SoftDelete(T entity)
        {
            _unitOfWork.GetRepository<T>().SoftDelete(entity);
            _unitOfWork.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _unitOfWork.GetRepository<T>().Update(entity);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
