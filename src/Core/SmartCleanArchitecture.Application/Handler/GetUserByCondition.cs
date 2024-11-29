using Cachelibrary.Interface;
using SmartCleanArchitecture.Application.Common.Cache;
using SmartCleanArchitecture.Application.Interfaces;
using SmartCleanArchitecture.Domain.Entities.Dals;
using SmartCleanArchitecture.Domain.Entities.Dtos;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Application.Handler
{
    public class GetUserByCondition: IGetUserByCondition
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        /// <summary>
        /// cache provider declaration
        /// </summary>
        private readonly ICacheProvider _cacheProvider;
        /// <summary>
        /// cache Options Provider declaration
        /// </summary>
        private readonly ICacheOptionsProvider<CacheEnum> _cacheOptionsProvider;

        public GetUserByCondition(IRepositoryWrapper repositoryWrapper, ICacheProvider cacheProvider, ICacheOptionsProvider<CacheEnum> cacheOptionsProvider)
        {
            _repositoryWrapper = repositoryWrapper;
            _cacheOptionsProvider = cacheOptionsProvider;
            _cacheProvider = cacheProvider;
        }

        public async Task<Users> GetUserByCondAsync(string condition)
        {
            var cacheOptions = await _cacheOptionsProvider.GetOptions(CacheEnum.User).ConfigureAwait(false);
            var cacheKey = $"{cacheOptions.Identifier}userData";

            /// <summary>
            /// get data from redis cache 
            /// </summary>
            var user  = await _cacheProvider.Get<Users>(cacheKey).ConfigureAwait(false);

            if (user == null)
            {
                var data = await _repositoryWrapper.UserRepository.FindByConditionAsync(usr => usr.Id.ToString() == condition || usr.UserName == condition || usr.Email == condition || usr.PhoneNo == condition);

                 user = data.FirstOrDefault();
                /// <summary>
                /// set data into the redis cache database
                /// </summary>
                await _cacheProvider.Set(cacheKey, user, cacheOptions).ConfigureAwait(false);

            }
           


            return user;
        }
    }
}
