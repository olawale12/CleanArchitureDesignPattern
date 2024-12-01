using Oracle.ManagedDataAccess.Client;
using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using System.Data;
using Dapper;
using SmartCleanArchitecture.Domain.Interfaces.IFactory;


namespace SmartCleanArchitecture.Infrastructure.Repository
{
    public class DapperRepositoryBase<TModel> : IDapperRepositoryBase<TModel> where TModel : class
    {
        private readonly IDbConnectionFactory _connectionFactory;


        public DapperRepositoryBase(IDbConnectionFactory connectionFactor)
        {
            _connectionFactory = connectionFactor;
        }


        #region Sample One of generic repository pattern for dapper
        /*
                public async Task<IEnumerable<TModel>> GetAllAsync()
                {
                    using (var connection = _connectionFactory.CreateConnection())
                    {
                         connection.Open();
                        return await connection.QueryAsync<TModel>($"SELECT * FROM {typeof(TModel).Name}");
                    }
                }

                public async Task<TModel> GetByIdAsync(int id)
                {
                    using (var connection = _connectionFactory.CreateConnection())
                    {
                        connection.Open();
                        return await connection.QuerySingleOrDefaultAsync<TModel>(
                            $"SELECT * FROM {typeof(TModel).Name} WHERE Id = :Id", new { Id = id });
                    }
                }

                public async Task AddAsync(TModel entity)
                {
                    using (var connection = _connectionFactory.CreateConnection())
                    {
                        connection.Open();
                        var insertQuery = $"INSERT INTO {typeof(TModel).Name} ( Add your columns here ) VALUES( Add your values here )";
                        await connection.ExecuteAsync(insertQuery, entity);
                    }
                }

                public async Task UpdateAsync(TModel entity)
                {
                    using (var connection = _connectionFactory.CreateConnection())
                    {
                        connection.Open();
                        var updateQuery = $"UPDATE {typeof(TModel).Name} SET  Add your columns and values here  WHERE Id = :Id";
                        await connection.ExecuteAsync(updateQuery, entity);
                    }
                }

                public async Task DeleteAsync(int id)
                {
                    using (var connection = _connectionFactory.CreateConnection())
                    {
                        connection.Open();
                        await connection.ExecuteAsync($"DELETE FROM {typeof(TModel).Name} WHERE Id = :Id", new { Id = id });
                    }
                } */

        #endregion

        public async Task<IEnumerable<TModel>> GetAllAsync(string sql)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryAsync<TModel>(sql);
            }
        }

        public async Task<TModel> GetByIdAsync(string sql, object parameters)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<TModel>(sql, parameters);
            }
        }

        public async Task AddAsync(string sql, object parameters)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task UpdateAsync(string sql, object parameters)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task DeleteAsync(string sql, object parameters)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
