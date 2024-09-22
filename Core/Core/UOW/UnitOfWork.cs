using Core.Data;
using Core.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task SaveAsync()
        {
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }

        public void Save()
        {
            if (_context.ChangeTracker.HasChanges())
            {
                var changesSaved = _context.SaveChanges() > 0;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
