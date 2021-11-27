using Infrastructure.Core.Repositories.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tables.Domain.Models;

namespace Tables.Infrastructure.Repository.Configuration
{
    public class TableConfiguration : BaseEntityTypeConfiguration<Table>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<Table> builder)
        {
            builder.HasIndex(nameof(Table.Name)).IsUnique();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.NumberOfPlaces).IsRequired();
            builder.HasCheckConstraint("CK_Tables_NumberOfPlaces", "\"NumberOfPlaces\" > '0'");
        }
    }
}