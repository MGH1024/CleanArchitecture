using Application.Services.Exception;
using Contract.Services.Public.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Config.Util.Filter
{
    public class ModelStateValidatorAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                List<ValidationMessage> validationFailures = new();

                var keys = context.ModelState.Keys;

                foreach (var key in keys)
                {
                    IEnumerable<string> errors = context.ModelState.Keys
                                                .Where(k => k == key)
                                                .Select(k => context.ModelState[k].Errors)
                                                .First()
                                                .Where(e => !string.IsNullOrEmpty(e.ErrorMessage))
                                                .Select(e => e.ErrorMessage);

                    if (errors.Any())
                    {
                        foreach (var error in errors)
                        {
                            string keyNormalize = key.Replace("$.", string.Empty);
                            string errorNormalize = key.StartsWith("$.") ? Messages.InvalidField : error;
                            validationFailures.Add(new ValidationMessage(Char.ToLowerInvariant(keyNormalize[0]) + keyNormalize[1..], errorNormalize));
                        }
                    }
                }

                throw new CustomValidationException(validationFailures);

            }

            await next();
        }
    }
}
