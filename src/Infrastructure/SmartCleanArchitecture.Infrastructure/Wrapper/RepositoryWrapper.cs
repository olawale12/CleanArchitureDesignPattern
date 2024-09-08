using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using SmartCleanArchitecture.Infrastructure.Data;
using SmartCleanArchitecture.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private SSODbContext _context;
        private IUserRepository _userRepository;
        public RepositoryWrapper(SSODbContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository
        {
            get {

                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
           
        }
    }
}
