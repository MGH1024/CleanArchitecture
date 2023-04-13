using Application.ConfigModels;
using Application.Services.Exception;
using Contract.Services.DatetimeProvider;
using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Contract.Models;
using Domain.Contract.Repositories;
using Domain.Entities.Identity;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Utility.AppSettingConfig;

namespace Application.Services.IdentityProvider;

public class IdentityService : IIdentityService
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IClaimService _claimService;
    private readonly ISignInService _signInService;
    private readonly Auth _auth;
    private readonly IDateTime _dateTime;
    private readonly IUserRep _userRep;
    private readonly IMapper _mapper;

    public IdentityService(IDateTime dateTime,
        IOptions<Auth> options,
        IUserService userService,
        IRoleService roleService,
        IClaimService claimService,
        ISignInService signInService,
        IUserRep userRep,
        IMapper mapper)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _claimService = claimService ?? throw new ArgumentNullException(nameof(claimService));
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
        _auth = options.Value;
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        _userRep = userRep ?? throw new ArgumentNullException(nameof(userRep));
        _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
    }

    public async Task<IEnumerable<User>> GetUsers(GetParameter getParameter)
    {

        return await _userRep.GetAllUsers(getParameter);
    }

    public async Task<IEnumerable<User>> GetUsersByShapingData(GetParameter getParameter)
    {
        return await _userRep.GetAllUsers(getParameter);
    }


    public async Task<User> GetUser(GetUserById getUserById)
    {
        return await _userRep.GetByIdAsync(getUserById.UserId);
    }

    public async Task<User> GetUser(int userId)
    {
        return await _userService.GetById(userId);
    }

    public async Task<List<string>> CreateUserSimple(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        var strResult = new List<string>();
        var userResult = await _userService.CreateUser(user);
        var roleResult = await _roleService.AddRoleToUser(user, (int)Roles.User);
        var claimResult = await _claimService.AddClaimToUser(user);
        if (userResult.Succeeded && roleResult.Succeeded && claimResult.Succeeded)
        {
            return strResult;
        }
        else
        {
            strResult.AddRange(GetIdentityError(userResult.Errors));
            strResult.AddRange(GetIdentityError(roleResult.Errors));
            strResult.AddRange(GetIdentityError(claimResult.Errors));
            return strResult;
        }
    }

    public async Task<List<string>> CreateUser(User user, string password)
    {
        var strResult = new List<string>();
        var userResult = await _userService.CreateUser(user, password);
        var roleResult = await _roleService.AddRoleToUser(user, (int)Roles.User);
        var claimResult = await _claimService.AddClaimToUser(user);
        if (userResult.Succeeded && roleResult.Succeeded && claimResult.Succeeded)
        {
            return strResult;
        }
        else
        {

            strResult.AddRange(GetIdentityError(userResult.Errors));
            strResult.AddRange(GetIdentityError(roleResult.Errors));
            strResult.AddRange(GetIdentityError(claimResult.Errors));
            return strResult;
        }
    }

    public async Task<List<string>> UpdateUser(UpdateUser updateUser)
    {
        var strResult = new List<string>();
        var user = await _userService.GetUserById(updateUser.Id);
        if (user != null)
        {
            //user
            UpdateUserProperty(user, updateUser);
            var userUpdateResult = await _userService.UpdateUser(user);

            //role
            var removeRoleResult = await _roleService.RemoveRolesByUser(user);
            var assignRoleToUser = await _roleService.AssignRolesToUser(user, updateUser.RoleIdList);

            //claim
            var removeClaimsResult = await _claimService.RemoveClaimsByUser(user);
            var assignClaimsToUser = await _claimService.AssignClaimsToUser(user, updateUser);

            if (userUpdateResult.Succeeded && removeRoleResult.Succeeded && assignRoleToUser.Succeeded
                && removeClaimsResult.Succeeded && assignClaimsToUser.Succeeded)
            {
                return strResult;
            }

            else
            {
                strResult.AddRange(GetIdentityError(userUpdateResult.Errors));
                strResult.AddRange(GetIdentityError(removeRoleResult.Errors));
                strResult.AddRange(GetIdentityError(assignRoleToUser.Errors));
                strResult.AddRange(GetIdentityError(removeClaimsResult.Errors));
                strResult.AddRange(GetIdentityError(assignClaimsToUser.Errors));
                return strResult;
            }
        }
        else
        {
            return strResult;
        }
    }

    public async Task<bool> IsInRole(int userId, int roleId)
    {
        var user = await _userService.GetById(userId);
        var role = await _roleService.GetById(roleId);
        return await _userService.IsUserInRole(user, role.Name);
    }

    public async Task<List<string>> DeleteUser(User user)
    {
        var strResult = new List<string>();
        var deleteUserResult = await _userService.DeleteUser(user);
        if (deleteUserResult.Succeeded)
            return strResult;
        else
            return GetIdentityError(deleteUserResult.Errors);
    }

    public async Task<LoginResult> Login(Login login, string ipAddress, string returnUrl)
    {
        var user = await _userService.GetByUsername(login.UserName);

        //2do if user signed in redirect to returnUrl
        var stringResult = new List<string>();

        if (user != null)
        {
            await _signInService.SignOut();
            var signinResult = await _signInService.SignIn(user, login);

            if (signinResult.IsNotAllowed)
            {
                if (!await _userService.IsEmailConfirmed(user))
                {
                    stringResult.Add("ایمیل تایید نشده است.");
                }

                if (!await _userService.IsPhoneNumberConfirmed(user))
                {
                    stringResult.Add("تلفن تایید نشده است.");
                }
            }

            if (signinResult.Succeeded)
            {
                //token
                var token = await GenerateTokenByUser(user);
                var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
                var tokenValidDate = _dateTime
                    .IranNow
                    .AddMinutes(_auth.TokenAddedExpirationDateValue);



                //refreshToken
                var refreshToken = GenerateRefreshToken();
                var refreshTokenValidDate = _dateTime
                    .IranNow
                    .AddMinutes(_auth.RefreshTokenAddedExpirationDateValue);
                await _userService.CreateUserRefreshToken(new UserRefreshToken
                {
                    CreatedDate = _dateTime.IranNow,
                    ExpirationDate = refreshTokenValidDate,
                    IpAddress = ipAddress,
                    IsInvalidated = false,
                    RefreshToken = refreshToken,
                    Token = tokenAsString,
                    UserId = user.Id
                });

                var loginResult = new LoginResult
                {
                    TokenValidDate = tokenValidDate,
                    Token = tokenAsString,
                    RefreshToken = refreshToken,
                    ReturnUrl = returnUrl
                };
                return loginResult;
            }

            if (signinResult.RequiresTwoFactor)
            {
                //2Do
            }

            if (signinResult.IsLockedOut)
            {
                stringResult.Add("اکانت شما به دلیل 5 بار ورود ناموفق به مدت 5 دقیقه لاک شده است .");
                return new LoginResult();
            }
        }

        stringResult.Add("کاربری با این مشخصات یافت نشد.");
        return new LoginResult();
    }

    public async Task<bool> IsEmailInUse(string email)
    {
        return await _userService.GetByEmail(email) != null;
    }

    public async Task<bool> IsUsernameInUse(string username)
    {
        return await _userService.GetByUsername(username) != null;
    }


    private void UpdateUserProperty(User user, UpdateUser updateUser)
    {
        user.Firstname = updateUser.Firstname;
        user.Lastname = updateUser.Lastname;
        user.Email = updateUser.Email;
        user.UserName = updateUser.Username;
        user.CellNumber = updateUser.CellNumber;
        user.Image = updateUser.Image;
        user.PhoneNumber = updateUser.PhoneNumber;
    }

    private List<string> GetIdentityError(IEnumerable<IdentityError> errors)
    {
        var strResult = new List<string>();
        foreach (var item in errors)
        {
            strResult.Add(item.Description);
        }
        return strResult;
    }

    private async Task<JwtSecurityToken> GenerateTokenByUser(User user)
    {
        var claims = await _signInService.GetClaimByUser(user);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth.AuthKey));

        return
            new JwtSecurityToken(
                issuer: _auth.AuthIssuer,
                audience: _auth.AuthAudinse,
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
    }


    private string GenerateRefreshToken()
    {
        var byteArray = new byte[32];
        using RNGCryptoServiceProvider cryptpProvider = new();
        cryptpProvider.GetBytes(byteArray);
        return Convert.ToBase64String(byteArray);
    }

    public string GetCurrentUser()
    {
        return "System";
    }

    public async Task<LoginResult> Refresh(RefreshToken refreshToken, string ipAddress)
    {
        if (refreshToken is null)
            throw new BadRequestException();

        var user = await _userService.GetUserByToken(new GetUserByToken { Token = refreshToken.Token });
        if (user is null)
            throw new BadRequestException();


        var userRefreshToken = _userRep.GetUserRefreshTokenByUserAndOldToken(user, refreshToken.Token, refreshToken.RefToken);
        if (userRefreshToken is null)
            throw new BadRequestException();


        var newToken = new JwtSecurityTokenHandler().WriteToken(await GenerateTokenByUser(user));
        var newTokenValidDate = _dateTime
            .IranNow
            .AddMinutes(_auth.TokenAddedExpirationDateValue);


        if (userRefreshToken.ExpirationDate < _dateTime.IranNow)
        {
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshTokenValidDate = _dateTime
                .IranNow.AddMinutes(_auth.RefreshTokenAddedExpirationDateValue);

            //deaciveOldRefreshtoken
            await _userService.DeactiveRefreshTokenRefreshToken(refreshToken.RefToken);


            //generate new and save in db
            await _userService.CreateUserRefreshToken(new UserRefreshToken
            {
                CreatedDate = _dateTime.IranNow,
                ExpirationDate = newRefreshTokenValidDate,
                IpAddress = ipAddress,
                IsInvalidated = false,
                RefreshToken = newRefreshToken,
                Token = newToken,
                UserId = user.Id
            });

            return new LoginResult()
            {
                Token = newToken,
                TokenValidDate = newTokenValidDate,
                RefreshToken = newRefreshToken
            };
        }
        else
        {
            return new LoginResult()
            {
                Token = newToken,
                TokenValidDate = newTokenValidDate,
                RefreshToken = refreshToken.RefToken,
            };

        }
    }
}