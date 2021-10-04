﻿using Microsoft.EntityFrameworkCore;
using Restaurant.Core.Domain;
using Restaurant.DataAccess.Configurations;

namespace Restaurant.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<KitchenOrder> KitchenOrders { get; set; }
        public DbSet<KitchenOrderDish> Dishes { get; set; }
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
