using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the ArtworkTag join entity.
/// It configures the join table between Artworks and Tags.
/// </summary>
public class ArtworkTagConfiguration : IEntityTypeConfiguration<ArtworkTag>
{
    public void Configure(EntityTypeBuilder<ArtworkTag> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ArtworkId)
            .IsRequired();
        builder.Property(e => e.TagId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relationship with Artwork
        builder.HasOne(e => e.Artwork)
            .WithMany(a => a.ArtworkTags) // Assumes Artwork has ICollection<ArtworkTag> ArtworkTags
            .HasForeignKey(e => e.ArtworkId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Deleting Artwork removes the tag association

        // Relationship with Tag
        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ArtworkTags) // Assumes Tag has ICollection<ArtworkTag> ArtworkTags
            .HasForeignKey(e => e.TagId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Deleting Tag removes the association from Artworks

        // Unique constraint for the combination
        builder.HasIndex(e => new { e.ArtworkId, e.TagId })
            .IsUnique();
    }
}