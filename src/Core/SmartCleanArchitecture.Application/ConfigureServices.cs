using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Application.Common.MessageProviders;
using SmartCleanArchitecture.Application.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cachelibrary;
using StackExchange.Redis;
using SmartCleanArchitecture.Application.Common.Cache;

namespace SmartCleanArchitecture.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(ctg =>
            {
                ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<IMessageProvider, MessageProvider>();
            services.AddSingleton<IMessageFullProvider, MessageFullProvider>();
            services.Configure<MessageFullSettings>(opt => configuration.GetSection("MessageFullSettings").Bind(opt));

            var cacheConfig = configuration.GetSection("Redis").Get<ConfigurationOptions>();


            var endPoints = configuration.GetSection("Redis:EndPoints")!.Get<string[]>()!.ToArray();

            var useSentinel = configuration.GetSection("Redis:UserSentinel")!.Get<bool>();

            var useRedisFlag = configuration.GetSection("Redis:UseRedis")!.Get<bool>();
            var encryptionKey = configuration.GetValue<string>("SystemSettings:EncryptionKey");
            var password = GeneralUtil.Decryptor(cacheConfig!.Password, encryptionKey);

            foreach (var item in endPoints)
            {
                cacheConfig!.EndPoints.Add(item);
            }

            services.AddCacheServices<CacheOptionsProvider, CacheEnum>(opt =>
            {

                foreach (var item in cacheConfig!.EndPoints)
                {
                    opt.EndPoints.Add(item);
                }
                opt.Password = password;

                if (useSentinel) opt.CommandMap = CommandMap.Sentinel;

                opt.AbortOnConnectFail = cacheConfig!.AbortOnConnectFail;
            }, useRedis: useRedisFlag);


            return services;
        }
    }
}
