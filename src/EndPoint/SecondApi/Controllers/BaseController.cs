using Contract.Services.Public.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace SecondAPi.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected string CurrentUser
    {
        get
        {
            var name =
                User.Claims
                .FirstOrDefault(x => x.Type.Equals("username", StringComparison.InvariantCultureIgnoreCase));
            if (name == null)
                return "";
            return name.Value;
        }
    }

    protected string IpAddress
    {
        get
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }

    protected IActionResult ApiResult<T>(T result)
    {
        var apiResult = new ApiResult<T>
        {
            Data = result
        };
        apiResult.Messages.Add(Messages.SuccessOperation);




        return Ok(apiResult);
    }

    protected IActionResult ApiResult()
    {
        var apiResult = new ApiResult<object>
        {
            Data = null
        };
        apiResult.Messages.Add(Messages.SuccessOperation);
        return Ok(apiResult);
    }

}
