using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the Tag entity.
/// It configures properties, keys, and constraints for the Tag table.
/// </summary>
public class TagConfiguration : IEntityTypeConfiguration<Tag> // Corrected class name if needed
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(255) // Using length from provided code example
            .IsRequired();
        builder.HasAlternateKey(e => e.Name); // Enforce unique tag names

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Note: 1:M relationships Tag -> ArtworkTag, Tag -> ReferenceTag, Tag -> UserFollowedTag
        // are configured here implicitly by the HasMany in this configuration
        // and explicitly by the HasOne in the respective join entity configurations.
    }
}