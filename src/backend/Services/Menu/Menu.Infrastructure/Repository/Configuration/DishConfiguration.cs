using Infrastructure.Core.Repositories.Configuration;
using Menu.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menu.Infrastructure.Repository.Configuration
{
    public class DishConfiguration : BaseEntityTypeConfiguration<Dish>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<Dish> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(300);
        }
    }
}