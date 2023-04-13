using Application.Services.Exception;
using Contract.Services.Public.DTOs;
using Domain.constant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;
using Utility;

namespace Config.Util.MiddleWares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorEventHandler> _logger;
    private readonly IHttpContextAccessor _accessor;
    private readonly IConfiguration _configuration;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorEventHandler> logger, IHttpContextAccessor accessor, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _accessor = accessor;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        string url = httpContext.Request.Path.Value;
        LogProperties logItem = null;

        if (new List<string>() { "/swagger", "/favicon.ico" }.Any(u => url.Contains(u.ToLower())))
        {
            await _next(httpContext);
        }
        else
        {
            Stream originalBody = httpContext.Response.Body;
            try
            {
                logItem = new LogProperties()
                {
                    Params = await GetRequestBodyAsync(httpContext),
                    Url = httpContext.Request.Host + url,
                    CorrelationId = Guid.NewGuid().ToString(),
                    Headers = string.Join(',', httpContext.Request.Headers.Select(h => $" {h.Key}:{h.Value}").ToList()),
                    ClientIP = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    ApplicationName = _configuration.GetSection("ApplicationName").Value,
                };

                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    logItem.ClientIP = httpContext.Request.Headers["X-Forwarded-For"];

                logItem.LogMessage = $"Request with code {logItem.CorrelationId} recived from {logItem.ClientIP}";

                _logger.LogInformation(JsonSerializer.Serialize(logItem).ToString());

                using (var memStream = new MemoryStream())
                {
                    httpContext.Response.Body = memStream;

                    await _next(httpContext);

                    memStream.Position = 0;
                    string response = await new StreamReader(memStream).ReadToEndAsync();
                    logItem.Response = response.Replace("\"", "'");
                    memStream.Position = 0;

                    await memStream.CopyToAsync(originalBody);
                }

                logItem.LogMessage = $"Request successfully done with code {logItem.CorrelationId}";
                _logger.LogInformation(JsonSerializer.Serialize(logItem).ToString());
            }
            catch (Exception ex)
            {
                httpContext.Response.Body = originalBody;
                await HandleExceptionAsync(httpContext, logItem, ex);
            }
        }
    }

    private static async Task<string> GetRequestBodyAsync(HttpContext httpContext)
    {
        var req = httpContext.Request;

        req.EnableBuffering();
        using var reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true);

        string bodyStr = await reader.ReadToEndAsync();
        req.Body.Position = 0;

        return bodyStr.Replace("\"", "'");
    }

    private async Task HandleExceptionAsync(HttpContext context, LogProperties logItem, Exception ex)
    {
        var httpStatusCode = HttpStatusCode.InternalServerError;

        ServiceException serviceException = null;

        StringBuilder messageStringBuilder = new();
        StringBuilder stackTraceStringBuilder = new();

        while (ex != null)
        {
            messageStringBuilder.Append($"Message: {ex.Message} ");
            stackTraceStringBuilder.Append($"Message: {ex.Message} ");

            if (ex is ServiceException serviceEx)
            {
                serviceException = serviceEx;
                messageStringBuilder.Append($"ApplicationExceptionMessage: { string.Join(',', serviceEx.ExceptionMessages) } ");
            }

            ex = ex.InnerException;
        }

        ErrorLog errorLog = new()
        {
            Message = messageStringBuilder.ToString(),
            StackTrace = stackTraceStringBuilder.ToString(),
        };

        string exceptionString = JsonSerializer.Serialize(errorLog, JsonSerializerSetting.JsonSerializerOptions);

        if (logItem != null)
        {
            logItem.LogMessage = $"Request failed with code {logItem.CorrelationId}";
            logItem.Exception = exceptionString;
        }
        else
        {
            string code = Guid.NewGuid().ToString();
            logItem = new LogProperties()
            {
                CorrelationId = code,
                LogMessage = $"Request failed with code {code}",
                Exception = exceptionString,
            };
        }

        if (serviceException != null)
        {
            httpStatusCode = serviceException.HttpStatusCode;
            _logger.LogWarning(JsonSerializer.Serialize(logItem).ToString());
        }
        else
            _logger.LogError(JsonSerializer.Serialize(logItem).ToString());


        context.Response.StatusCode = (int)httpStatusCode;
        context.Response.ContentType = "application/json";

        var apiResult = new ApiResult<object>();

        if (serviceException != null)
        {
            if (serviceException is CustomValidationException validationException)
                apiResult.ValidationMessages.AddRange(validationException.ValidationErrors);

            apiResult.Messages.AddRange(serviceException.ExceptionMessages);
        }
        else
            apiResult.Messages.AddRange(new string[] { Messages.UnknownException, "Tracking Code: " + logItem?.CorrelationId ?? "" });

        string jsonstring = JsonSerializer.Serialize(apiResult, JsonSerializerSetting.JsonSerializerOptions);

        await context.Response.WriteAsync(jsonstring, Encoding.UTF8);

        // to stop futher pipeline execution 
        return;
    }
}