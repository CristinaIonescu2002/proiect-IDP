using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the Medium entity.
/// It configures properties, keys, and constraints for the Medium table.
/// </summary>
public class MediumConfiguration : IEntityTypeConfiguration<Medium>
{
    public void Configure(EntityTypeBuilder<Medium> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(100) // Adjust length as needed
            .IsRequired();
        builder.HasAlternateKey(e => e.Name); // Ensure Medium names are unique

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // The relationship with Artwork (1:M Medium -> Artworks) is configured
        // primarily via the HasOne in ArtworkConfiguration pointing to Medium.
        // Could add builder.HasMany(m => m.Artworks).WithOne(a => a.Medium)... here as well if preferred.
    }
}