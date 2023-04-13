﻿using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Contract.Repositories;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Services.IdentityProvider;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRep _userRep;

    public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IUserRep userRep)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _userRep = userRep ?? throw new ArgumentNullException(nameof(userRep));
    }

    public async Task<IdentityResult> CreateUser(User user)
    {
        return await _userManager.CreateAsync(user);
    }

    public async Task<IdentityResult> CreateUser(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<User> GetUserById(int userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<IdentityResult> UpdateUser(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<User> GetCurrentUser()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var name = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(x => x.Type.Equals("username", StringComparison.InvariantCultureIgnoreCase));

            if (name is null)
                return null;
            return await _userManager.FindByNameAsync(name.Value);
        }
        return null;
    }

    public async Task<User> GetById(int userId)
    {
        return await _userManager.Users.FirstAsync(u => u.Id == userId);
    }

    public async Task<User> GetByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<bool> IsUserInRole(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<bool> IsEmailConfirmed(User user)
    {
        return await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<bool> IsPhoneNumberConfirmed(User user)
    {
        return await _userManager.IsPhoneNumberConfirmedAsync(user);
    }

    public async Task<IdentityResult> DeleteUser(User user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task CreateUserRefreshToken(UserRefreshToken userRefreshToken)
    {
        await _userRep.InsertUserRefreshToken(userRefreshToken);
    }

    public Task<User> GetUserByToken(GetUserByToken getUserByToken)
    {
        var username = GetUserNameClaimBytoken(getUserByToken.Token);
        return GetByUsername(username);
    }

    public async Task DeactiveRefreshTokenRefreshToken(string refreshToken)
    {
        await _userRep.InvalidateRefreshToken(refreshToken);
    }

    private string GetUserNameClaimBytoken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        return jwtSecurityToken.Claims.First(claim => claim.Type == "username").Value;
    }
}