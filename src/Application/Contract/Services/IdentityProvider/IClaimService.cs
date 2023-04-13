using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Contract.Services.IdentityProvider;

public interface IClaimService
{
    Task<IdentityResult> AddClaimToUser(User user);
    Task<IdentityResult> RemoveClaimsByUser(User user);
    Task<IdentityResult> AssignClaimsToUser(User user, UpdateUser updateUser);
}