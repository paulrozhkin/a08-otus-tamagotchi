using Infrastructure.Core.Repositories.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderQueue.Core.Models;

namespace OrderQueue.DataAccess.Configuration
{
    public class KitchenOrderConfiguration : BaseEntityTypeConfiguration<KitchenOrder>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<KitchenOrder> builder)
        {
        }
    }
}