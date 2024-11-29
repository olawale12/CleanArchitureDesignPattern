using SmartCleanArchitecture.Domain.Entities.Dals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Interfaces
{
    public interface IGetUserByCondition
    {
        Task<Users> GetUserByCondAsync(string condition);
    }
}
