using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartCleanArchitecture.Infrastructure.IRepository;
using SmartCleanArchitecture.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<UsersTest> _Users;
        IConfiguration _configuration;
        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IGenericRepository<UsersTest> Users => _Users ??= new GenericRepository<UsersTest>(_configuration);
       
    }
}
