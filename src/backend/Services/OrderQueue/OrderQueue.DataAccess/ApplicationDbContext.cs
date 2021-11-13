using Microsoft.EntityFrameworkCore;
using OrderQueue.Core.Domain;

namespace OrderQueue.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KitchenOrder> KitchenOrders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }
    }
}
