using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq; // For Select, Any, Contains etc.
using Microsoft.EntityFrameworkCore; // For Include/ThenInclude

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter and project Reference entities to ReferenceDTO.
/// Includes necessary related data and defines the Select projection.
/// </summary>
public sealed class ReferenceProjectionSpec : Specification<Reference, ReferenceDTO>
{
    /// <summary>
    /// Defines the projection logic from Reference entity to ReferenceDTO.
    /// Includes related User, Tags, and Artworks (as simple DTOs).
    /// </summary>
    private static System.Linq.Expressions.Expression<Func<Reference, ReferenceDTO>> Projection =>
        r => new ReferenceDTO
        {
            Id = r.Id,
            Title = r.Title, // Assuming entity uses Title
            Description = r.Description,
            ImagePath = r.ImagePath,
            UserId = r.UserId,
            // Project nested UserSimpleDTO (Requires Include)
            User = r.User == null ? null : new UserSimpleDTO
            {
                Id = r.User.Id,
                Name = r.User.Name,
                Email = r.User.Email
            },
            // Project nested Tags (Requires Include)
            Tags = r.ReferenceTags.Select(rt => new TagDTO
            {
                Id = rt.Tag.Id,
                Name = rt.Tag.Name
            }).ToList(),
            // Project nested Artworks (Requires Include)
            UsedInArtworks = r.ArtworkReferences.Select(ar => new ArtworkSimpleDTO
            {
                Id = ar.Artwork.Id,
                Title = ar.Artwork.Title,
                ImagePath = ar.Artwork.ImagePath,
                User = ar.Artwork.User == null ? null : new UserSimpleDTO // Requires deeper include
                {
                    Id = ar.Artwork.User.Id,
                    Name = ar.Artwork.User.Name,
                    Email = ar.Artwork.User.Email
                }
            }).ToList()
            // Add other properties like CreatedAt/UpdatedAt if they are in ReferenceDTO
            // CreatedAt = r.CreatedAt,
            // UpdatedAt = r.UpdatedAt
        };

    /// <summary>
    /// Specification for projecting a single Reference by ID to ReferenceDTO.
    /// </summary>
    public ReferenceProjectionSpec(Guid id)
    {
        Query.Where(r => r.Id == id);

        // Add includes BEFORE the Select. Adjust depth as necessary.
        Query.Include(r => r.User);
        Query.Include(r => r.ReferenceTags).ThenInclude(rt => rt.Tag);
        Query.Include(r => r.ArtworkReferences).ThenInclude(ar => ar.Artwork).ThenInclude(a => a.User); // Include Artworks->User

        // Apply the projection
        Query.Select(Projection);
    }

    /// <summary>
    /// Specification for projecting a list of References to ReferenceDTO, with optional filters.
    /// </summary>
    public ReferenceProjectionSpec(string? search = null, Guid? userId = null, List<Guid>? tagIds = null)
    {
        // Add includes BEFORE the Select and potentially before Where if filtering on included data.
        Query.Include(r => r.User);
        Query.Include(r => r.ReferenceTags).ThenInclude(rt => rt.Tag);
        // Note: Including ArtworkReferences->Artwork->User for list projections can be VERY heavy.
        // Consider if UsedInArtworks is really needed in the LIST DTO or only in the single item DTO.
        // If needed, uncomment the include below:
         Query.Include(r => r.ArtworkReferences).ThenInclude(ar => ar.Artwork).ThenInclude(a => a.User);

        // Apply Filters
        if (!string.IsNullOrWhiteSpace(search))
        {
            Query.Where(r => r.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            (r.Description != null && r.Description.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                            r.User.Name.Contains(search, StringComparison.OrdinalIgnoreCase)); // Requires User include
        }
        if (userId.HasValue)
        {
            Query.Where(r => r.UserId == userId.Value);
        }
        if (tagIds != null && tagIds.Any())
        {
            // Requires ALL tags
            foreach (var tagId in tagIds)
            {
                Query.Where(r => r.ReferenceTags.Any(rt => rt.TagId == tagId));
            }
        }

        // Apply the projection
        Query.Select(Projection);

        Query.OrderByDescending(r => r.CreatedAt); // Default sort
    }
}