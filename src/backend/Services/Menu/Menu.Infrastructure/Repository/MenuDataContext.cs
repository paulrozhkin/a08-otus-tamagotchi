using System.Reflection;
using Menu.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Menu.Infrastructure.Repository
{
    public class MenuDataContext: DbContext
    {
        public MenuDataContext(DbContextOptions<MenuDataContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
