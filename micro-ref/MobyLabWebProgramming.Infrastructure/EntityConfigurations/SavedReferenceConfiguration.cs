using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the SavedReference join entity.
/// It configures the join table between Users and the References they saved.
/// </summary>
public class SavedReferenceConfiguration : IEntityTypeConfiguration<SavedReference>
{
    public void Configure(EntityTypeBuilder<SavedReference> builder)
    {
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.ReferenceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();

        // Relationship with User
        builder.HasOne(sr => sr.User)
            .WithMany(u => u.SavedReferences) // Assumes User has ICollection<SavedReference> SavedReferences
            .HasForeignKey(sr => sr.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // If user deleted, their saved records are gone

        // Relationship with Reference
        builder.HasOne(sr => sr.Reference)
            .WithMany(r => r.SavingUsers) // Assumes Reference has ICollection<SavedReference> SavingUsers
            .HasForeignKey(sr => sr.ReferenceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // If reference deleted, saved records are gone

        // Unique constraint for the combination
        builder.HasIndex(e => new { e.UserId, e.ReferenceId })
            .IsUnique();
    }
}