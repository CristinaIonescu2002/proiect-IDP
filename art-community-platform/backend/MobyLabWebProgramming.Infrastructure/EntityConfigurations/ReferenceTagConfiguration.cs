using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the ReferenceTag join entity.
/// It configures the join table between References and Tags.
/// </summary>
public class ReferenceTagConfiguration : IEntityTypeConfiguration<ReferenceTag>
{
    public void Configure(EntityTypeBuilder<ReferenceTag> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ReferenceId)
            .IsRequired();
        builder.Property(e => e.TagId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relationship with Reference
        builder.HasOne(e => e.Reference)
            .WithMany(r => r.ReferenceTags) // Assumes Reference has ICollection<ReferenceTag> ReferenceTags
            .HasForeignKey(e => e.ReferenceId)
            .IsRequired()
            .OnDelete(DeleteBehavior. Cascade); // Deleting Reference removes the tag association

        // Relationship with Tag
        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ReferenceTags) // Assumes Tag has ICollection<ReferenceTag> ReferenceTags
            .HasForeignKey(e => e.TagId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Deleting Tag removes the association from References

        // Unique constraint for the combination
        builder.HasIndex(e => new { e.ReferenceId, e.TagId })
            .IsUnique();
    }
}