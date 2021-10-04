using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Core.Domain;

namespace Restaurant.DataAccess.Configurations
{
    public class KitchenOrderDishConfiguration : IEntityTypeConfiguration<KitchenOrderDish>
    {
        public void Configure(EntityTypeBuilder<KitchenOrderDish> builder)
        {
            builder
                .ToTable("KitchenOrderDishes")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.MenuId)
                .IsRequired();

            builder
                .HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.DishStatusId);
        }
    }
}
