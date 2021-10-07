using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderQueue.Core.Abstractions.Repositories;
using OrderQueue.DataAccess.Repositories;

namespace OrderQueue.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString) => services
            .AddDbContext<ApplicationDbContext>(x =>
            {
                x.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                );
                x.UseLazyLoadingProxies();
            });

        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
    }
}
