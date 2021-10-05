using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderQueue.Core.Domain;

namespace OrderQueue.DataAccess.Configurations
{
    public class DishStatusConfiguration : IEntityTypeConfiguration<DishStatus>
    {
        public void Configure(EntityTypeBuilder<DishStatus> builder)
        {
            builder
                .ToTable("DishStatuses")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
