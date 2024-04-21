using System;
using System.Threading.Tasks;
using MinhTuan.Domain.Core.Repository;
using MinhTuan.Domain.Entities;

namespace MinhTuan.Domain.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VStyleContext _context;

        public UnitOfWork(VStyleContext context, IRepository<Product> productRepo)
        {
            _context = context;
            ProductRepo = productRepo;
        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }
        public IRepository<Product> ProductRepo { get; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
