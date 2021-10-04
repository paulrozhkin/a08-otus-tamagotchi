using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Restaurant.API.Extensions;
using Restaurant.DataAccess;

namespace Restaurant.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().MigrateDatabase<ApplicationDbContext>().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
