using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks; // Thêm namespace này
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Core.UnitOfWork;
using MinhTuan.Service.Core.Services;

namespace MinhTuan.Service.Core
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(T entity) // Đổi thành async Task
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _unitOfWork.GetRepository<T>().Add(entity);
            await _unitOfWork.SaveChangesAsync(); // Sử dụng await
        }

        public async Task DeleteAsync(T entity) // Đổi thành async Task
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _unitOfWork.GetRepository<T>().Delete(entity);
            await _unitOfWork.SaveChangesAsync(); // Sử dụng await
        }

        public async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate) // Đổi thành async Task<IEnumerable<T>>
        {
            // Kiểm tra xem phương thức FindBy trong Repository đã hỗ trợ async chưa
            // Nếu chưa, bạn có thể chuyển đổi hoặc sử dụng Task.Run để bọc phương thức đồng bộ
            return await Task.Run(() => _unitOfWork.GetRepository<T>().FindBy(predicate));
        }

        public async Task<T?> GetByIdAsync(Guid? id) // Đổi thành async Task<T?>
        {
            // Tương tự như FindByAsync, kiểm tra xem GetById đã hỗ trợ async chưa
            return await Task.Run(() => _unitOfWork.GetRepository<T>().GetById(id));
        }

        public IQueryable<T> GetQueryable() // Không cần đổi vì không có tác vụ bất đồng bộ
        {
            return _unitOfWork.GetRepository<T>().GetQueryable();
        }

        public async Task SoftDeleteAsync(T entity) // Đổi thành async Task
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _unitOfWork.GetRepository<T>().SoftDelete(entity);
            await _unitOfWork.SaveChangesAsync(); // Sử dụng await
        }

        public async Task UpdateAsync(T entity) // Đổi thành async Task
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _unitOfWork.GetRepository<T>().Update(entity);
            await _unitOfWork.SaveChangesAsync(); // Sử dụng await
        }
    }
}
