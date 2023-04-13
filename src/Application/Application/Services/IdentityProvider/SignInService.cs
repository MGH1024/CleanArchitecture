using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services.IdentityProvider;

public class SignInService : ISignInService
{
    private readonly SignInManager<User> _signInManager;

    public SignInService(SignInManager<User> signInManager)
    {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<SignInResult> SignIn(User user, Login login)
    {
        return await _signInManager.PasswordSignInAsync
            (user.UserName,
            login.Password,
            login.RememberMe,
            lockoutOnFailure: true);
    }

    public async Task<IEnumerable<Claim>> GetClaimByUser(User user)
    {
        return await _signInManager.UserManager.GetClaimsAsync(user);
    }
}