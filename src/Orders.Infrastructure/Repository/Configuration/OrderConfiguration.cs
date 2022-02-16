using Infrastructure.Core.Repositories.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Models;

namespace Orders.Infrastructure.Repository.Configuration
{
    public class OrderConfiguration : BaseEntityTypeConfiguration<Order>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<Order> builder)
        {
        }
    }
}