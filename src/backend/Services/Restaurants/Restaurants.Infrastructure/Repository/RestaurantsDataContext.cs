using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Models;

namespace Restaurants.Infrastructure.Repository
{
    public class RestaurantsDataContext : DbContext
    {
        public RestaurantsDataContext(DbContextOptions<RestaurantsDataContext> options) : base(options)
        {
        }
        
        public DbSet<Restaurant> Restaurants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}