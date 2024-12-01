using SmartCleanArchitecture.Domain.Interfaces.IFactory;
using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using SmartCleanArchitecture.Infrastructure.Repository;


namespace SmartCleanArchitecture.Infrastructure.Wrapper
{
    public class DapperRepositoryWrapper : IDapperRepositoryWrapper
    {
        private IDbConnectionFactory _connectionFactory;
        private IUserDapperRepository _userRepository;

        public DapperRepositoryWrapper(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IUserDapperRepository UserDapperRepository 
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserDapperRepository(_connectionFactory);
                }
              return  _userRepository;
            }
        }
    }
}
