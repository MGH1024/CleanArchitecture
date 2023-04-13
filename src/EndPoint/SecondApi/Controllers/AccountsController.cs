using SecondAPi.Controllers;
using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace SecondApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : BaseController
{
    private readonly IIdentityService _identityService;
    private readonly IUserService _userService;

    public AccountsController(IIdentityService identityService, IUserService userService)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Login login, string returnUrl)
    {
        var result = await _identityService.Login(login, IpAddress, returnUrl);
        return Ok(result);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(RefreshToken refreshToken)
    {
        var result = await _identityService.Refresh(refreshToken, IpAddress);
        return Ok(result);
    }

    [HttpGet]
    [Route("getuserbytoken")]
    public async Task<IActionResult> GetUserNameByToken([FromQuery] GetUserByToken getUserByToken)
    {
        var user = await _userService.GetUserByToken(getUserByToken);
        return ApiResult(user.UserName);
    }
}