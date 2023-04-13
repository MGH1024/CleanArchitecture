using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace Utility.RouteConstraints
{
    /// <summary>
    /// چک کردن اینکه کالچر ورودی معتبر است یا نه
    /// </summary>
    public class CultureRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var isMatch = false;

            if (values.ContainsKey("culture"))
            {
                var cultureRouteValue = values["culture"].ToString();

                var isCultureValid = CultureInfo
                    .GetCultures(CultureTypes.AllCultures)
                    .Any(culture => string.Equals(culture.Name, cultureRouteValue, StringComparison.OrdinalIgnoreCase));

                if (isCultureValid)
                {
                    isMatch = true;
                }
            }

            return isMatch;
        }


    }
}
