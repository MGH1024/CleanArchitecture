using APi.Controllers;
using AutoMapper;
using Contract.Services.IdentityProvider;
using Contract.Services.IdentityProvider.DTOs.User;
using Domain.Contract.Models;
using Domain.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public UsersController(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        _mapper = mapper;
    }

    [HttpGet("getusers")]
    public async Task<IActionResult> GetUsers([FromQuery] GetParameter resourceParameter)
    {
        var result = await _identityService.GetUsers(resourceParameter);
        return ApiResult(result);
    }

    [HttpGet("getusersbyshapingdata")]
    public async Task<IActionResult> GetUsersByShapingData([FromQuery] GetParameter resourceParameter)
    {
        var result = await _identityService.GetUsersByShapingData(resourceParameter);
        return ApiResult(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _identityService.GetUser(new GetUserById { UserId = id });
        return ApiResult(result);
    }

    [HttpPost("createuser")]
    public async Task<IActionResult> CreateUser(CreateUser createUser)
    {
        var user = _mapper.Map<User>(createUser);
        var result = await _identityService.CreateUser(user, createUser.Password);
        return ApiResult(result);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(UpdateUser updateUser)
    {
        var result = await _identityService.UpdateUser(updateUser);
        return ApiResult(result);
    }
}