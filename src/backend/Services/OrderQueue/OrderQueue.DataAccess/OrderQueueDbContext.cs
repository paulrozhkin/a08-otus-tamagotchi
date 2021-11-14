using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrderQueue.Core.Models;

namespace OrderQueue.DataAccess
{
    public class OrderQueueDbContext : DbContext
    {
        public DbSet<KitchenOrder> KitchenOrders { get; set; }

        public OrderQueueDbContext(DbContextOptions<OrderQueueDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}