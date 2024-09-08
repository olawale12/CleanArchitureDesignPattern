using SmartCleanArchitecture.Domain.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Domain.Interfaces.IWrapper
{
    public interface IRepositoryWrapper
    {
        IUserRepository UserRepository { get; }
        
    }
}
