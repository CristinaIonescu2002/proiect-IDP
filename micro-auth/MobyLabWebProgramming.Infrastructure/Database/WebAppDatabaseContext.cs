using Ardalis.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;

namespace MobyLabWebProgramming.Infrastructure.Database;

/// <summary>
/// This is the database context used to connect with the database and links the ORM, Entity Framework, with it.
/// </summary>
public sealed class WebAppDatabaseContext : DbContext
{
    public WebAppDatabaseContext(DbContextOptions<WebAppDatabaseContext> options, bool migrate = true) : base(options)
    {
        if (migrate)
        {
            Database.Migrate();
        }
    }

    public DbSet<Medium> Mediums { get; set; } = default!;
    public DbSet<Artwork> Artworks { get; set; } = default!;
    public DbSet<Reference> References { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<ArtworkTag> ArtworkTags { get; set; } = default!;
    public DbSet<ReferenceTag> ReferenceTags { get; set; } = default!;
    public DbSet<ArtworkReference> ArtworkReferences { get; set; } = default!;

    /// <summary>
    /// Here additional configuration for the ORM is performed.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("unaccent")
            .ApplyAllConfigurationsFromCurrentAssembly(); // Here all the classes that contain implement IEntityTypeConfiguration<T> are searched at runtime
                                                          // such that each entity that needs to be mapped to the database tables is configured accordingly.
    }
}