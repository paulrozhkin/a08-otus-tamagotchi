using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Core.Domain;

namespace Restaurant.DataAccess.Configurations
{
    public class KitchenOrderStatusConfiguration : IEntityTypeConfiguration<KitchenOrderStatus>
    {
        public void Configure(EntityTypeBuilder<KitchenOrderStatus> builder)
        {
            builder
                .ToTable("KitchenOrderStatuses")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
