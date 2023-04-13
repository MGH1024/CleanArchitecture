using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services.IdentityProvider;

public class ClaimService : IClaimService
{
    private readonly UserManager<User> _userManager;

    public ClaimService(UserManager<User> userManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IdentityResult> AddClaimToUser(User user)
    {
        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.GivenName,user.Firstname),
                            new Claim(ClaimTypes.Surname,user.Lastname)
                        };
        return await _userManager.AddClaimsAsync(user, claims);
    }

    public async Task<IdentityResult> RemoveClaimsByUser(User user)
    {
        var oldClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.GivenName,user.Firstname),
                        new Claim(ClaimTypes.Surname,user.Lastname)
                    };
        return await _userManager.RemoveClaimsAsync(user, oldClaims);
    }

    public async Task<IdentityResult> AssignClaimsToUser(User user, UpdateUser updateUser)
    {
        var newClaims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Email,updateUser.Email),
                                    new Claim(ClaimTypes.GivenName,updateUser.Firstname),
                                    new Claim(ClaimTypes.Surname,updateUser.Lastname)
                                };
        return await _userManager.AddClaimsAsync(user, newClaims);
    }
}

