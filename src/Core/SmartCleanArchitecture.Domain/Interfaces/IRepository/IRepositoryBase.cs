using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Domain.Interfaces.IRepository
{
    public interface IRepositoryBase<TModel> where TModel : class
    {
        Task<TModel> FindAsync(string ID);
        Task<IQueryable<TModel>> FindAllAsync();
        Task<IEnumerable<TModel>> FindByConditionAsync(Expression<Func<TModel, bool>> expression);
        Task<IEnumerable<TModel>> FindByConditionAsyncAsNoTracking(Expression<Func<TModel, bool>> expression);
        Task CreateAsync(TModel entity);
        void Update(TModel entity);
        void Delete(TModel entity);
        Task SaveAsync();
    }
}
