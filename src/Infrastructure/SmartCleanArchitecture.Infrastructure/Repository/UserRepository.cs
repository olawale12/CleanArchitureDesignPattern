using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using SmartCleanArchitecture.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public UserRepository(SSODbContext context)
            :base(context)
        {
            
        }
    }
}
