using Microsoft.EntityFrameworkCore;
using MinhTuan.Domain.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MinhTuan.Domain.Core.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VStyleContext _context;
        private DbSet<T> _dbSet;

        public Repository(VStyleContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual DbSet<T> DBSet()
        {
            return _dbSet;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbSet.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public virtual T Delete(T entity)
        {
            return _dbSet.Remove(entity).Entity;
        }

        public void SoftDelete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Kiểm tra xem entity có thực sự implement SoftDelete không (nếu có)
            var softDeletable = entity as IAuditableEntity;
            if (softDeletable != null)
            {
                softDeletable.IsDelete = true; // Đánh dấu entity là đã bị xóa mềm
                
            }
            else
            {
                throw new InvalidOperationException("Entity must implement ISoftDeletable interface for soft delete operation");
            }
        }

        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            //foreach (var item in entities)
            //{
            //    if (_context.Entry(item).State == EntityState.Detached)
            //    {
            //        _dbSet.Remove(item);
            //    }
            //    _dbSet.Remove(item);
            //}
        }
        public void SoftDeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var softDeletable = entity as IAuditableEntity;
                if (softDeletable != null)
                {
                    softDeletable.IsDelete = true; // Đánh dấu entity là đã bị xóa mềm

                }
            }
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            //foreach (var item in entities)
            //{
            //    if (_context.Entry(item).State == EntityState.Detached)
            //    {
            //        _dbSet.Attach(item);
            //    }
            //    _dbSet.Add(item);
            //}
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _dbSet.Attach(item);
                }
                _dbSet.Update(item);
            }
        }
       
    }
}
