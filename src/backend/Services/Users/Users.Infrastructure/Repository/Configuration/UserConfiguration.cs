using Infrastructure.Core.Repositories.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Models;

namespace Users.Infrastructure.Repository.Configuration
{
    public class UserConfiguration : BaseEntityTypeConfiguration<User>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Roles).IsRequired();

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasMany(s => s.Roles)
                .WithMany(c => c.Users);
        }
    }
}