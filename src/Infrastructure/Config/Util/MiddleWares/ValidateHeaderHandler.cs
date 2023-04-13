using Contract.Services.ExternalServices;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Config.Util.MiddleWares
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        private readonly ISecondApiService _secondApiService;

        public ValidateHeaderHandler(ISecondApiService secondApiService)
        {
            _secondApiService = secondApiService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri.AbsolutePath;
            if (new List<string>() { "/getweather" }.Any(u => requestUri.Contains(u.ToLower())))
            {
                var token = await _secondApiService.Login();
                request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
