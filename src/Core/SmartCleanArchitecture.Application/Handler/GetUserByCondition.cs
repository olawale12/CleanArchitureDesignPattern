using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Handler
{
    public class GetUserByCondition
    {

        private readonly IRepositoryWrapper _repositoryWrapper;

        public GetUserByCondition(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<Users> GetUserByCondAsync(string condition)
        {

            var data = await _repositoryWrapper.UserRepository.FindByConditionAsync(usr => usr.Id.ToString() == condition || usr.UserName == condition || usr.Email == condition || usr.PhoneNo == condition);

            var user = data.FirstOrDefault();
            return user;
        }
    }
}
