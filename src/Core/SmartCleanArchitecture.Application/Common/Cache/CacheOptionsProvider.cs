
using Cachelibrary.Interface;
using Cachelibrary.Models;

namespace SmartCleanArchitecture.Application.Common.Cache
{
    public class CacheOptionsProvider : ICacheOptionsProvider<CacheEnum>
    {
        readonly IDictionary<CacheEnum, CacheOptions> _cacheOptions = new Dictionary<CacheEnum, CacheOptions>();

        private readonly int secondsInAday = 86400;
        private readonly int secondsInHalfHour = 1800;
        private readonly int secondsInHour = 3600;
        private readonly int secondsInHalf = 30;

        public CacheOptionsProvider()
        {
            _cacheOptions.Add(CacheEnum.User, new CacheOptions()
            {
                Identifier = "PaymentService",
                AbsoluteExpirySeconds = secondsInAday,
                SlidingExpirySeconds = secondsInAday
            });
           

           
        }


        public async Task<CacheOptions> GetOptions(CacheEnum key)
        {
            if (!_cacheOptions.ContainsKey(key))
                throw new Exception("Cache item not found");

            return await Task.FromResult(_cacheOptions[key]);
        }
    }
}
