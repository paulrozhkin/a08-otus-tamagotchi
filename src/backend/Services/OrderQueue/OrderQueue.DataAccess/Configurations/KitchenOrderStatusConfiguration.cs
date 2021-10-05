using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderQueue.Core.Domain;

namespace OrderQueue.DataAccess.Configurations
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
