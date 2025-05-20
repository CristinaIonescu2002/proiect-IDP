using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to project Artworks using a specific Reference to ArtworkSimpleDTO.
/// Requires Select implementation in repository or evaluator.
/// </summary>
public sealed class ArtworksUsingReferenceProjectionSpec : Specification<Artwork, ArtworkSimpleDTO>
{
    public ArtworksUsingReferenceProjectionSpec(Guid referenceId)
    {
        // Filter artworks that have an ArtworkReference matching the referenceId
        Query.Where(a => a.ArtworkReferences.Any(ar => ar.ReferenceId == referenceId))
            .Include(a => a.User); // Include User for the DTO projection

        // Project to ArtworkSimpleDTO
        Query.Select(a => new ArtworkSimpleDTO
        {
            Id = a.Id,
            Title = a.Title,
            ImagePath = a.ImagePath,
            User = new UserSimpleDTO // Requires User to be included
            {
                Id = a.UserId,
                Name = a.User.Name,
                Email = a.User.Email
            }
        });
        Query.OrderByDescending(a => a.UploadDate); // Add default sort
    }
}