using APi.Controllers;
using Contract.Services.ExternalServices;
using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : BaseController
{
    private readonly IIdentityService _identityService;
    private readonly IUserService _userService;
    private readonly ISecondApiService _secondApiService;

    public AccountsController(IIdentityService identityService, IUserService userService, ISecondApiService secondApiService)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _secondApiService = secondApiService ?? throw new ArgumentNullException(nameof(secondApiService));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(Login login, string returnUrl)
    {
        var result = await _identityService.Login(login, IpAddress, returnUrl);
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto createUserDto)
    {
        var result = await _identityService.CreateUserSimple(createUserDto);
        return Ok(result);
    }

    [HttpGet("weatherforecast-callby-polly")]
    public async Task<IActionResult> CallByPolly()
    {
        var result = await _secondApiService.WeatherForecastPolly();
        return Ok(result);
    }

    [HttpGet("weatherforecast-callby-httpclientfactory")]
    public async Task<IActionResult> CallByHttpClientFactory()
    {
        var result = await _secondApiService.WeatherForecastClientFactory();
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
    [Route("get-user-by-token")]
    public async Task<IActionResult> GetUserNameByToken([FromQuery] GetUserByToken getUserByToken)
    {
        var user = await _userService.GetUserByToken(getUserByToken);
        return ApiResult(user.UserName);
    }
}