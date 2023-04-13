using Contract.Services.CachingProvider;
using Contract.Services.ExternalServices;
using Contract.Services.IdentityProvider.DTOs.User;
using Contract.Services.Public.DTOs;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Config.Util.MiddleWares
{
    public class SetTokenMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ISecondApiService _secondApiService;
        private readonly ICachingService<LoginResult> _cachingService;

        public SetTokenMiddleWare(RequestDelegate next, ISecondApiService secondApiService, ICachingService<LoginResult> cachingService)
        {
            _next = next;
            _secondApiService = secondApiService;
            _cachingService = cachingService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string url = context.Request.Path.Value;
            if (new List<string>() { "/Weather" }.Any(u => url.Contains(u.ToLower())))
            {
                var token = await _secondApiService.Login();
                context.Request.Headers.Add("Authorization", "Bearer " + token.Token);
                await _next(context);
            }
            await _next(context);
        }
    }
}
