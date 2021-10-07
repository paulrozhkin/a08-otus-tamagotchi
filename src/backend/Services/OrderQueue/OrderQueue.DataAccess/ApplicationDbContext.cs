using Microsoft.EntityFrameworkCore;
using OrderQueue.Core.Domain;
using OrderQueue.DataAccess.Configurations;

namespace OrderQueue.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KitchenOrder> KitchenOrders { get; set; }
        public DbSet<KitchenOrderDish> Dishes { get; set; }
        public DbSet<KitchenOrderStatus> KitchenOrderStatuses { get; set; }
        public DbSet<DishStatus> DishStatuses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DishStatusConfiguration());
            builder.ApplyConfiguration(new KitchenOrderConfiguration());
            builder.ApplyConfiguration(new KitchenOrderDishConfiguration());
            builder.ApplyConfiguration(new KitchenOrderStatusConfiguration());
        }
    }
}
