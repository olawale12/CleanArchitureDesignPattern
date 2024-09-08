using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using SmartCleanArchitecture.Infrastructure.Data;
using SmartCleanArchitecture.Infrastructure.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            string conStr = configuration.GetConnectionString("SSODbConnection");
            services.AddDbContext<SSODbContext>(opt => { }

              //  opt.UseNpgsql(conStr ?? throw new InvalidOperationException("SSO Db connection not found"))
            );

            services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
            return services;
        }
    }
}
