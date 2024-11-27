using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.IRepository
{
    public interface IUnitOfWork
    {
     
        IGenericRepository<UsersTest> Users { get; }
       
    }
}
