using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;

namespace MinhTuan.Domain.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //Khai báo Repository được sử dụng
       
        IRepository<Product> ProductRepo { get; }

        //Kết thúc khai báo
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}