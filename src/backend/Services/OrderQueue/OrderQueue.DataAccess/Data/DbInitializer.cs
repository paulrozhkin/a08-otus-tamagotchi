namespace OrderQueue.DataAccess.Data
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
        }
    }
}
