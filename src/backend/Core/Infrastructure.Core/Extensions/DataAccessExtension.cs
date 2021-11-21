using Domain.Core.Repositories;
using Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Core.Extensions
{
    public static class DataAccessExtension
    {
        public static IServiceCollection AddDataAccess<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();

            services
                .AddDbContext<TDbContext>(x =>
                {
                    x.UseNpgsql(
                        connectionString,
                        b => b.MigrationsAssembly(typeof(TDbContext).Assembly.FullName)
                    );
                });

            return services;
        }
    }
}