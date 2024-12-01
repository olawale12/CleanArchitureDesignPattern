using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCleanArchitecture.Domain.Interfaces.IFactory;
using SmartCleanArchitecture.Domain.Interfaces.IWrapper;
using SmartCleanArchitecture.Infrastructure.Data;
using SmartCleanArchitecture.Infrastructure.Wrapper;

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

            string connectionString = configuration.GetConnectionString("OracleDb");
            services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(connectionString));
            services.AddScoped<IDapperRepositoryWrapper, DapperRepositoryWrapper>();

             services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
            return services;
        }
    }
}
