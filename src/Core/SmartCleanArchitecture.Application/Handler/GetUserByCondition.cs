using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Handler
{
    public class GetUserByCondition
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IConfiguration _configuration;
        private readonly OracleConnection _connection;
        public GetUserByCondition(IConfiguration configuration)
        {
           // _repositoryWrapper = repositoryWrapper;
            _connection = new OracleConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Users> GetUserByCondAsync(string condition)
        {

            //var data = await _repositoryWrapper.UserRepository.FindByConditionAsync(usr => usr.Id.ToString() == condition || usr.UserName == condition || usr.Email == condition || usr.PhoneNo == condition);
            //var user = data.FirstOrDefault();           
            var data = await _connection.QueryAsync<Users>($"SELECT *  FROM USERS where USERNAME ='{condition}' or EMAIL ='{condition}' or LASTNAME='{condition}'");            
            var user = data.FirstOrDefault();
            return user;
        }
    }
}
