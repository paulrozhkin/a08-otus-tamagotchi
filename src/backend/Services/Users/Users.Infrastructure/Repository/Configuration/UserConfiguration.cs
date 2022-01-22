using Domain.Core.Models;
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

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.HasMany(s => s.Roles)
                .WithMany(c => c.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleUser",
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    je =>
                    {
                        je.HasKey("UserId", "RoleId");
                        je.HasData(new {UserId = Guid.Parse("A95F884A-BE44-411C-BC7E-C65E01BFB022"), RoleId = Guid.Parse("922CCD71-6928-47D2-AEE5-1FECDE2DE951") });
                    });

            builder.HasData(new User
            {
                Id = Guid.Parse("A95F884A-BE44-411C-BC7E-C65E01BFB022"),
                Name = "admin",
                Username = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin")
            });
        }
    }


    public class RoleConfiguration : BaseEntityTypeConfiguration<Role>
    {
        protected override void ConfigureOtherProperties(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new()
            {
                Id = Guid.Parse("922CCD71-6928-47D2-AEE5-1FECDE2DE951"),
                Name = Roles.Administrator
            }, new()
            {
                Id = Guid.Parse("C452E9BA-5C25-4C80-9652-A109EFD6E47A"),
                Name = Roles.Client
            }, new()
            {
                Id = Guid.Parse("894B85D0-C90A-43CD-A5F6-488545ED2A4E"),
                Name = Roles.Stuff
            });
        }
    }
}