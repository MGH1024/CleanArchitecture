using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Contract.Services.IdentityProvider;

public interface ISignInService
{
    Task SignOut();
    Task<SignInResult> SignIn(User user, Login login);
    Task<IEnumerable<Claim>> GetClaimByUser(User user);
}