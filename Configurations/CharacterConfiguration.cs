using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiRPG.Models;

namespace WebApiRPG.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder
            .HasOne(c => c.Weapon)
            .WithOne(w => w.Character)
            .HasForeignKey<Weapon>(w => w.CharacterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasMany(c => c.Skills)
            .WithMany(s => s.Characters);
            
    }
}
