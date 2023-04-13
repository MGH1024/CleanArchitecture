using APi.Controllers;
using Application.Services.Exception;
using AutoMapper;
using Contract.Services.Public;
using Contract.Services.Public.DTOs.State;
using Domain.Contract.Models;
using Domain.Entities.Public;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Utility;

namespace Api.Controllers;

[ApiController]
[Route("{culture:CultureRouteConstraint}/Api/[Controller]")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class StatesController : BaseController
{
    private readonly IStateService _stateService;

    public StatesController(IStateService stateService)
    {
        _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetStates([FromQuery] GetParameter resourceParameter)
    {
        var list = await _stateService.GetStatesAsync(resourceParameter);
        return ApiResult(list);
    }

    [HttpGet]
    [Route("getactivestates")]
    public async Task<IActionResult> GetActiveStates([FromQuery] GetParameter resourceParameter)
    {
        var list = await _stateService.GetActiveStatesAsync(resourceParameter);
        return ApiResult(list);
    }

    [HttpGet]
    [Route("getupdatedstates")]
    public async Task<IActionResult> GetUpdatedStates([FromQuery] GetParameter resourceParameter)
    {
        var list = await _stateService.GetUpdatedStatesAsync(resourceParameter);
        return ApiResult(list);
    }

    [HttpGet]
    [Route("getdeletedstates")]
    public async Task<IActionResult> GetDeletedStates([FromQuery] GetParameter resourceParameter)
    {
        var list = await _stateService.GetDeletedStatesAsync(resourceParameter);
        return ApiResult(list);
    }

    [HttpGet]
    [Route("{Id}")]
    public async Task<IActionResult> GetState([FromQuery] GetStateById getStateById)
    {
        var result = await _stateService.GetStateAsync(getStateById);
        return ApiResult(result);
    }


    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateState(CreateState createState)
    {
        await _stateService.CreateStateAsync(createState);
        return ApiResult();
    }


    [HttpPut]
    [Route("")]
    public async Task<IActionResult> UpdateState(UpdateState updateState)
    {
        await _stateService.UpdateStateAsync(updateState);
        return ApiResult();
    }

    [HttpDelete]
    [Route("")]
    public async Task<IActionResult> DeleteState(DeleteState deleteState)
    {   
        await _stateService.DeleteStateAsync(deleteState);
        return ApiResult();
    }
}