using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Needed for Any/Contains/Select

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter and project Artwork entities to ArtworkDTO.
/// Requires Select implementation in repository or evaluator.
/// </summary>
public sealed class ArtworkProjectionSpec : Specification<Artwork, ArtworkDTO>
{
    // Common includes needed for projection - adjust as needed
    private static IQueryable<Artwork> AddIncludes(IQueryable<Artwork> query) =>
        query.Include(a => a.User)
             .Include(a => a.Medium)
             .Include(a => a.ArtworkTags).ThenInclude(at => at.Tag)
             .Include(a => a.ArtworkReferences).ThenInclude(ar => ar.Reference).ThenInclude(r => r.User); // Include deeply for nested DTOs

    // Common projection logic
    private static System.Linq.Expressions.Expression<Func<Artwork, ArtworkDTO>> Projection =>
        a => new ArtworkDTO
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            UploadDate = a.UploadDate,
            ImagePath = a.ImagePath,
            UserId = a.UserId,
            User = new UserSimpleDTO // Project nested User
            {
                Id = a.User.Id,
                Name = a.User.Name,
                Email = a.User.Email
            },
            MediumId = a.MediumId,
            Medium = new MediumDTO // Project nested Medium
            {
                Id = a.Medium.Id,
                Name = a.Medium.Name
            },
            Tags = a.ArtworkTags.Select(at => new TagDTO // Project nested Tags
            {
                Id = at.Tag.Id,
                Name = at.Tag.Name
            }).ToList(),
            References = a.ArtworkReferences.Select(ar => new ReferenceSimpleDTO // Project nested Refs
            {
                Id = ar.Reference.Id,
                Title = ar.Reference.Title, // Or Name
                ImagePath = ar.Reference.ImagePath,
                User = new UserSimpleDTO // Nested user in reference
                {
                    Id = ar.Reference.User.Id,
                    Name = ar.Reference.User.Name,
                    Email = ar.Reference.User.Email
                }
            }).ToList()
        };


    /// <summary> Projects single Artwork by ID. </summary>
    public ArtworkProjectionSpec(Guid id)
    {
         Query.Where(a => a.Id == id);
         Query.Select(Projection); // Apply the projection
         // Note: Includes should ideally be applied BEFORE the Select if possible by the evaluator/repo
         // Query = AddIncludes(Query); // Apply includes (might need adjustment based on repo/evaluator)
    }

    /// <summary> Projects Artwork list with filters. </summary>
    public ArtworkProjectionSpec(string? search = null, Guid? userId = null, Guid? mediumId = null, List<Guid>? tagIds = null)
    {
         // Apply Includes first if needed for filtering or Select
         // Query = AddIncludes(Query); // Apply includes (might need adjustment based on repo/evaluator)

         // Apply Filters
         if (!string.IsNullOrWhiteSpace(search))
         {
             Query.Where(a => a.Title.Contains(search) || (a.Description != null && a.Description.Contains(search)) || a.User.Name.Contains(search));
         }
         if (userId.HasValue)
         {
             Query.Where(a => a.UserId == userId.Value);
         }
         if (mediumId.HasValue)
         {
             Query.Where(a => a.MediumId == mediumId.Value);
         }
         if (tagIds != null && tagIds.Any())
         {
             // Requires ALL tags
             foreach (var tagId in tagIds) { Query.Where(a => a.ArtworkTags.Any(at => at.TagId == tagId)); }
         }

         // Apply Projection
         Query.Select(Projection);

         Query.OrderByDescending(a => a.UploadDate); // Add default sort
    }
}