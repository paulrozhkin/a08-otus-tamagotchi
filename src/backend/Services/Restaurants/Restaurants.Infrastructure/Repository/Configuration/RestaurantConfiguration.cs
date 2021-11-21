using Infrastructure.Core.Repositories.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.Domain.Models;

namespace Restaurants.Infrastructure.Repository.Configuration
{
    public class RestaurantConfiguration : BaseEntityTypeConfiguration<Restaurant>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<Restaurant> builder)
        {
            builder.Ignore(x => x.Address);
        }
    }
}