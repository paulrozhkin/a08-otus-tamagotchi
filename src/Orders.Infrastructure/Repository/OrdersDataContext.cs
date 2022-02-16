using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Models;

namespace Orders.Infrastructure.Repository
{
    public class OrdersDataContext : DbContext
    {
        public OrdersDataContext(DbContextOptions<OrdersDataContext> options) : base(options)
        {
        }
        
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}