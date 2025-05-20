using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the Artwork entity.
/// It configures properties, keys, and relationships for the Artwork table.
/// </summary>
public class ArtworkConfiguration : IEntityTypeConfiguration<Artwork>
{
    public void Configure(EntityTypeBuilder<Artwork> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(4095); // Nullable by default if string?

        builder.Property(e => e.UploadDate)
            .IsRequired();

        builder.Property(e => e.ImagePath)
            .HasMaxLength(255) // Consider if longer paths are needed
            .IsRequired();

        // Foreign Key for User
        builder.Property(e => e.UserId)
            .IsRequired();

        // Foreign Key for Medium (Added)
        builder.Property(e => e.MediumId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relationship with User (Creator)
        builder.HasOne(e => e.User)
            .WithMany(u => u.Artworks) // Assumes User has ICollection<Artwork> Artworks
            .HasForeignKey(e => e.UserId)
            // .HasPrincipalKey(e => e.Id) // Usually not needed if FK points to PK 'Id'
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Deleting User deletes their Artworks

        // Relationship with Medium (Added)
        builder.HasOne(e => e.Medium)
            .WithMany(m => m.Artworks) // Assumes Medium has ICollection<Artwork> Artworks
            .HasForeignKey(e => e.MediumId)
            // .HasPrincipalKey(m => m.Id) // Usually not needed
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Medium from deleting Artworks

        // Note: 1:M relationships Artwork -> ArtworkTag and Artwork -> ArtworkReference
        // are configured here implicitly by the HasMany in this configuration
        // and explicitly by the HasOne in ArtworkTagConfiguration/ArtworkReferenceConfiguration.
    }
}