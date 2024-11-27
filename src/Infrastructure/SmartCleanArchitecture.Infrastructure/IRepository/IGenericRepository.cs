using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get(string selectQuery);
        Task<IEnumerable<T>> Find(string selectQuery);
        Task<T> Add(T model);
        Task<T> Update(T model);
        Task<int> Remove(T model);
    }
}
