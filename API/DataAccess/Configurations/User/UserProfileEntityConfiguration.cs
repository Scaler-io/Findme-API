using API.Entities;
using API.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configurations.User
{
    public class UserProfileentityConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired();
            builder.Property(p => p.LastName)
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithOne(u => u.Profile);

            builder.HasMany(p => p.Photos)
                .WithOne(i => i.Profile)
                .HasForeignKey(fk => fk.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Address)
                .WithOne(a => a.Profile)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Gender)
                .HasConversion(
                    o => o.ToString(), 
                    o => (Gender)Enum.Parse(typeof(Gender), o)
                );
        }
    }
}
