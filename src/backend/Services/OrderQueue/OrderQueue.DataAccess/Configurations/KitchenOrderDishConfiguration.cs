using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderQueue.Core.Domain;

namespace OrderQueue.DataAccess.Configurations
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
                .Property(x => x.DishStatusId)
                .HasDefaultValue(1);

            builder
                .HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.DishStatusId);
        }
    }
}
