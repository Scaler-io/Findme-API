using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configurations.User
{
    public class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.UserName)
                .IsRequired();

            builder.HasIndex(u => u.UserName).IsUnique();

            builder.HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}
