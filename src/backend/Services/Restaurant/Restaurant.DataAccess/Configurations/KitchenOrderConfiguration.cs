using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Core.Domain;

namespace Restaurant.DataAccess.Configurations
{
    public class KitchenOrderConfiguration : IEntityTypeConfiguration<KitchenOrder>
    {
        public void Configure(EntityTypeBuilder<KitchenOrder> builder)
        {
            builder
                .ToTable("KitchenOrders")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.OrderId)
                .IsRequired();

            builder
                .Property(x => x.CreateTime)
                .IsRequired();

            builder
                .HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.KitchenOrderStatusId);

            builder
                .HasMany(x => x.Dishes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
