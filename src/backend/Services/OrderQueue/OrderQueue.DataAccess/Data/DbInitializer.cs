namespace OrderQueue.DataAccess.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly OrderQueueDbContext _context;

        public DbInitializer(OrderQueueDbContext context)
        {
            _context = context;
        }

        public void Init()
        {
            _context.Database.EnsureCreated();
        }
    }
}