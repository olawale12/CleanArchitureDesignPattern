using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Interfaces.IFactory;
using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Repository
{
    public class UserDapperRepository : DapperRepositoryBase<Users>, IUserDapperRepository
    {
        public UserDapperRepository(IDbConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<Users>> GetAllProductsAsync()
        {
            string sql = "SELECT * FROM Users";
            return await GetAllAsync(sql);
        }

        public async Task<Users> GetProductByIdAsync(int id)
        {
            string sql = "SELECT * FROM Users WHERE Id = :Id";
            return await GetByIdAsync(sql, new { Id = id });
        }

        public async Task AddProductAsync(Users users)
        {
            string sql = "INSERT INTO Users (Name, Price) VALUES (:Name, :Price)";
            await AddAsync(sql, users);
        }

        public async Task UpdateProductAsync(Users users)
        {
            string sql = "UPDATE Products SET Name = :Name, Price = :Price WHERE Id = :Id";
            await UpdateAsync(sql, users);
        }

        public async Task DeleteProductAsync(int id)
        {
            string sql = "DELETE FROM Products WHERE Id = :Id";
            await DeleteAsync(sql, new { Id = id });
        }
    }
}
