
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Service.Core.Services;

namespace MinhTuan.Service.Core
{
    public class Service<T> : IService<T> where T : class
    {
        private  readonly IUnitOfWork _unitOfWork;
      

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _unitOfWork.GetRepository<T>().Add(entity);
            _unitOfWork.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
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

            if (entity == null) throw new ArgumentNullException("entity");
            _unitOfWork.GetRepository<T>().SoftDelete(entity);
            _unitOfWork.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _unitOfWork.GetRepository<T>().Update(entity);
            _unitOfWork.SaveChangesAsync();
        }
    }
}
