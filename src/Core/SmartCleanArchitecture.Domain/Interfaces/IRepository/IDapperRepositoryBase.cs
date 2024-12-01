using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Domain.Interfaces.IRepository
{
    public interface IDapperRepositoryBase<TModel> where TModel : class
    {
        #region sample one interface
        /*
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(int id);
        Task AddAsync(TModel entity);
        Task UpdateAsync(TModel entity);
        Task DeleteAsync(int id);
        */
        #endregion

        Task<IEnumerable<TModel>> GetAllAsync(string sql);
        Task<TModel> GetByIdAsync(string sql, object parameters);
        Task AddAsync(string sql, object parameters);
        Task UpdateAsync(string sql, object parameters);
        Task DeleteAsync(string sql, object parameters);
    }
}
