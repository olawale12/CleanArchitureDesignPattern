using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using SmartCleanArchitecture.Infrastructure.IRepository;
using SmartCleanArchitecture.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IConfiguration _configuration;
        private readonly OracleConnection _connection;
        public GenericRepository(IConfiguration configuration)
        {           
            _configuration = configuration;          
            _connection = new OracleConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<T> Add(T model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> Find(string selectQuery)
        {
            return await _connection.QueryAsync<T>(selectQuery);
        }

        public async Task<IEnumerable<T>> Get(string selectQuery)
        {
          
            return await _connection.QueryAsync<T>(selectQuery);
        }

        public async Task<int> Remove(T model)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
