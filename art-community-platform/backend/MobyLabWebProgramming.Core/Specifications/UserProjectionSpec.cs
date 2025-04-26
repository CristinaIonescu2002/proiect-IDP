using Ardalis.Specification;
using Microsoft.EntityFrameworkCore; // For Include/ThenInclude
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq; // For Select, Contains, Any etc.

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter and project User entities to UserDTO.
/// Requires Select implementation in repository or evaluator.
/// </summary>
public sealed class UserProjectionSpec : Specification<User, UserDTO> // Specify output type UserDTO
{
    /// <summary>
    /// Constructor for projecting a SINGLE User by ID to a DETAILED UserDTO.
    /// Includes all necessary relationships for the full DTO.
    /// </summary>
    public UserProjectionSpec(Guid id)
    {
        Query.Where(e => e.Id == id);

        // Includes needed for the DETAILED UserDTO
        Query.Include(u => u.Following);
        Query.Include(u => u.Followers);
        Query.Include(u => u.Artworks).ThenInclude(a => a.User);
        Query.Include(u => u.SavedReferences).ThenInclude(sr => sr.Reference).ThenInclude(r => r.User);
        Query.Include(u => u.FollowedTags).ThenInclude(uft => uft.Tag);
        Query.Include(u => u.References);


        // Define the projection to the DETAILED UserDTO
        Query.Select(e => new UserDTO
        {
            Id = e.Id,
            Email = e.Email,
            Name = e.Name,
            Role = e.Role,
            // Map included collections to DTOs/SimpleDTOs
            Artworks = e.Artworks.Select(a => new ArtworkSimpleDTO {
                 Id = a.Id, Title = a.Title, ImagePath = a.ImagePath,
                 User = a.User == null ? null : new UserSimpleDTO { Id = a.User.Id, Name = a.User.Name, Email = a.User.Email }
            }).ToList(),
            Following = e.Following.Select(f => new UserSimpleDTO {
                 Id = f.Id, Name = f.Name, Email = f.Email
            }).ToList(),
            Followers = e.Followers.Select(f => new UserSimpleDTO {
                 Id = f.Id, Name = f.Name, Email = f.Email
            }).ToList(),
             SavedReferences = e.SavedReferences.Where(sr => sr.Reference != null).Select(sr => new ReferenceSimpleDTO {
                 Id = sr.Reference.Id, Title = sr.Reference.Title, ImagePath = sr.Reference.ImagePath,
                 User = sr.Reference.User == null ? null : new UserSimpleDTO { Id = sr.Reference.UserId, Name = sr.Reference.User.Name, Email = sr.Reference.User.Email }
             }).ToList(),
             FollowedTags = e.FollowedTags.Where(uft => uft.Tag != null).Select(uft => new TagDTO {
                 Id = uft.TagId, Name = uft.Tag.Name
             }).ToList(),
            References = e.References.Select(r => new ReferenceSimpleDTO {
                Id = r.Id, Title = r.Title, ImagePath = r.ImagePath,
                User = new UserSimpleDTO { Id = e.Id, Name = e.Name, Email = e.Email }
            }).ToList(),
            // Map counts
            ArtworkCount = e.Artworks.Count,
            FollowerCount = e.Followers.Count,
            FollowingCount = e.Following.Count,
            SavedReferenceCount = e.SavedReferences.Count,
            FollowedTagCount = e.FollowedTags.Count,
            ReferenceCount = e.References.Count
        });
    }

    /// <summary>
    /// Constructor for projecting a LIST of Users to UserDTO, with optional search filter.
    /// WARNING: This version includes projecting ALL related entity lists (Artworks, References, Followers, etc.)
    /// for every user, which can be VERY INEFFICIENT and lead to large responses. Use with caution.
    /// </summary>
    public UserProjectionSpec(string? search = null)
    {
        // Apply filtering FIRST
        if (!string.IsNullOrWhiteSpace(search))
        {
             Query.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                             e.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        // Apply includes needed for the projection SECOND
        // Include ALL relationships needed for the full UserDTO projection
        Query.Include(u => u.Following);
        Query.Include(u => u.Followers);
        Query.Include(u => u.Artworks).ThenInclude(a => a.User);
        Query.Include(u => u.SavedReferences).ThenInclude(sr => sr.Reference).ThenInclude(r => r.User);
        Query.Include(u => u.FollowedTags).ThenInclude(uft => uft.Tag);
        Query.Include(u => u.References);

        // Define the projection to UserDTO THIRD
        // Includes mapping for ALL lists as requested.
        Query.Select(e => new UserDTO
        {
            Id = e.Id,
            Email = e.Email,
            Name = e.Name,
            Role = e.Role,
            // Project ALL Lists
            Artworks = e.Artworks.Select(a => new ArtworkSimpleDTO {
                 Id = a.Id, Title = a.Title, ImagePath = a.ImagePath,
                 User = a.User == null ? null : new UserSimpleDTO { Id = a.User.Id, Name = a.User.Name, Email = a.User.Email }
            }).ToList(),
            References = e.References.Select(r => new ReferenceSimpleDTO {
                Id = r.Id, Title = r.Title, ImagePath = r.ImagePath,
                User = new UserSimpleDTO { Id = e.Id, Name = e.Name, Email = e.Email }
            }).ToList(),
            Following = e.Following.Select(f => new UserSimpleDTO {
                 Id = f.Id, Name = f.Name, Email = f.Email
            }).ToList(),
            Followers = e.Followers.Select(f => new UserSimpleDTO {
                 Id = f.Id, Name = f.Name, Email = f.Email
            }).ToList(),
             SavedReferences = e.SavedReferences.Where(sr => sr.Reference != null).Select(sr => new ReferenceSimpleDTO {
                 Id = sr.Reference.Id, Title = sr.Reference.Title, ImagePath = sr.Reference.ImagePath,
                 User = sr.Reference.User == null ? null : new UserSimpleDTO { Id = sr.Reference.UserId, Name = sr.Reference.User.Name, Email = sr.Reference.User.Email }
             }).ToList(),
             FollowedTags = e.FollowedTags.Where(uft => uft.Tag != null).Select(uft => new TagDTO {
                 Id = uft.TagId, Name = uft.Tag.Name
             }).ToList(),
            // Map COUNTS (still useful)
            ArtworkCount = e.Artworks.Count,
            FollowerCount = e.Followers.Count,
            FollowingCount = e.Following.Count,
            SavedReferenceCount = e.SavedReferences.Count,
            FollowedTagCount = e.FollowedTags.Count,
            ReferenceCount = e.References.Count
        });

        // Apply Ordering LAST
        Query.OrderBy(u => u.Name); // Example default order
    }
}