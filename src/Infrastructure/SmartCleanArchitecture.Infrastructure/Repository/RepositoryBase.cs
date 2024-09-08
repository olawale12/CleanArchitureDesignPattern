using Microsoft.EntityFrameworkCore;
using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using SmartCleanArchitecture.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TModel> : IRepositoryBase<TModel> where TModel : class
    {
        private readonly SSODbContext _context;

        public RepositoryBase(SSODbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(TModel entity)
        {
            await _context.Set<TModel>().AddAsync(entity);
        }

        public void Delete(TModel entity)
        {
            
            var property = entity.GetType().GetProperty("IsDeleted");
            if (property != null && property.PropertyType == typeof(bool))
            {
                property.SetValue(entity, true);
                Update(entity);
            }
            else
            {
                _context.Set<TModel>().Remove(entity);
            }
        }

        public async Task<IQueryable<TModel>> FindAllAsync()
        {
            return _context.Set<TModel>().AsNoTracking();
        }

        public async Task<TModel> FindAsync(string ID)
        {
            return await _context.Set<TModel>().FindAsync(ID);
        }

        public async Task<IEnumerable<TModel>> FindByConditionAsync(Expression<Func<TModel, bool>> expression)
        {
            return await _context.Set<TModel>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TModel>> FindByConditionAsyncAsNoTracking(Expression<Func<TModel, bool>> expression)
        {
            return await _context.Set<TModel>().AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(TModel entity)
        {
            _context.Set<TModel>().Update(entity);
        }
    }
}
