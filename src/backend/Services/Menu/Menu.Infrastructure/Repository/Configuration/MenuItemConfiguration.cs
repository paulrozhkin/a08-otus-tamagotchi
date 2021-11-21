using Infrastructure.Core.Repositories.Configuration;
using Menu.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Menu.Infrastructure.Repository.Configuration
{
    public class MenuItemConfiguration : BaseEntityTypeConfiguration<MenuItem>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("Menu");

            builder.Property(x => x.PriceRubles).IsRequired();
            builder.Property(x => x.DishId).IsRequired();
            builder.Property(x => x.RestaurantId).IsRequired();

            builder.HasCheckConstraint("CK_Menu_PriceRubles", "\"PriceRubles\" > '0'");

            builder.HasOne(x => x.Dish).WithMany(x => x.Menu);
        }
    }
}