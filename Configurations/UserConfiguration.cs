using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiRPG.Models;

namespace WebApiRPG.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(x => x.Username)
            .IsUnique();
        builder
            .HasIndex(x => x.EmailAddress)
            .IsUnique();      
    }
}
