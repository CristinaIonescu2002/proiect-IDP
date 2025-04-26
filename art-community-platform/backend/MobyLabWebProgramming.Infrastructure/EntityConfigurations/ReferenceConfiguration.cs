using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the Reference entity.
/// It configures properties, keys, and relationships for the Reference table.
/// </summary>
public class ReferenceConfiguration : IEntityTypeConfiguration<Reference>
{
    public void Configure(EntityTypeBuilder<Reference> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        // Assuming Name was changed to Title for consistency
        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(4095);

        builder.Property(e => e.ImagePath)
            .HasMaxLength(255) // Consider if longer paths are needed
            .IsRequired();

        // Foreign Key for User (Creator)
        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relationship with User (Uploader)
        builder.HasOne(e => e.User)
            .WithMany(u => u.References) // Assumes User has ICollection<Reference> References
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Deleting User deletes their References

        // Note: 1:M relationships Reference -> ReferenceTag, Reference -> ArtworkReference,
        // Reference -> SavedReference are configured here implicitly by the HasMany
        // and explicitly by the HasOne in the respective join entity configurations.
    }
}