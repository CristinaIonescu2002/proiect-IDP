using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using System.Collections.Generic; // Needed for UsingEntity dictionary

namespace MobyLabWebProgramming.Infrastructure.EntityConfigurations;

/// <summary>
/// This is the entity configuration for the User entity.
/// It configures properties, keys, relationships, and constraints for the User table.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User> // Using provided code structure
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary Key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Name).HasMaxLength(255).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
        builder.Property(u => u.Password).HasMaxLength(255).IsRequired(); // Remember to store HASHED passwords
        builder.Property(u => u.Role).HasConversion(new EnumToStringConverter<UserRoleEnum>()).HasMaxLength(255).IsRequired(); // MaxLength should fit longest enum string
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();

        // Unique Constraints
        builder.HasAlternateKey(u => u.Email);

        // Relationships

        // One-to-Many: User -> Artworks
        builder.HasMany(u => u.Artworks)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .IsRequired() // An artwork must have a creator
            .OnDelete(DeleteBehavior.Cascade); // Deleting user deletes their artworks

        // One-to-Many: User -> References (Uploaded)
        builder.HasMany(u => u.References)
             .WithOne(r => r.User)
             .HasForeignKey(r => r.UserId)
             .IsRequired() // A reference must have a creator
             .OnDelete(DeleteBehavior.Cascade); // Deleting user deletes their references

        // One-to-Many: User -> UserFiles (From provided example)
        // Configuration moved to UserFileConfiguration for clarity, but could be here too.
        // builder.HasMany(e => e.UserFiles).WithOne(e => e.User).HasForeignKey(e => e.UserId)...

        // One-to-Many: User -> SavedReferences (Join Entity)
        builder.HasMany(u => u.SavedReferences)
            .WithOne(sr => sr.User)
            .HasForeignKey(sr => sr.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-Many: User -> UserFollowedTags (Join Entity)
        builder.HasMany(u => u.FollowedTags)
            .WithOne(uft => uft.User)
            .HasForeignKey(uft => uft.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many: User -> User (Following/Followers)
        builder.HasMany(u => u.Following)
            .WithMany(u => u.Followers)
            .UsingEntity<Dictionary<string, object>>(
                "UserFollows", // Explicit Join table name
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("FollowingId")
                    .OnDelete(DeleteBehavior.Restrict), // Prevent cascade delete issues
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("FollowerId")
                    .OnDelete(DeleteBehavior.Cascade), // If follower deleted, remove follow record
                j =>
                {
                    j.HasKey("FollowerId", "FollowingId");
                    // ToTable("UserFollows"); // Redundant if name specified in UsingEntity
                });
    }
}