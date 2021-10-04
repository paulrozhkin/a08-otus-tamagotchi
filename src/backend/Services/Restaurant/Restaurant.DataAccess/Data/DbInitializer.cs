using System.Linq;

namespace Restaurant.DataAccess.Data
{
    public class DbInitializer : IDbInitializer
    {
        private ApplicationDbContext _context { get; set; }

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Init()
        {
            _context.Database.EnsureCreated();

            if (!_context.KitchenOrderStatuses.Any())
            {
                _context.KitchenOrderStatuses.AddRange(FakeDataFactory.KitchenOrderStatuses);
                _context.SaveChanges();
            }

            if (!_context.DishStatuses.Any())
            {
                _context.DishStatuses.AddRange(FakeDataFactory.DishStatuses);
                _context.SaveChanges();
            }
        }
    }
}
