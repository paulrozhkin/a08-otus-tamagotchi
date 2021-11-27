using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tables.Domain.Models;

namespace Tables.Infrastructure.Repository
{
    public class TablesDataContext : DbContext
    {
        public TablesDataContext(DbContextOptions<TablesDataContext> options) : base(options)
        {
        }

        public DbSet<Table> Tables { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}