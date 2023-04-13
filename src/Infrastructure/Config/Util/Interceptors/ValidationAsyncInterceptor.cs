using Castle.DynamicProxy;
using Utility;
using FluentValidation;
using Contract.Services.Public.DTOs;
using Application.Services.Exception;

namespace Config.Util.Interceptors
{
    public class ValidationAsyncInterceptor : AsyncInterceptorBase
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationAsyncInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await ValidateObjects(invocation);

            await proceed(invocation, proceedInfo).ConfigureAwait(false);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            await ValidateObjects(invocation);

            return await proceed(invocation, proceedInfo).ConfigureAwait(false);
        }

        private async Task ValidateObjects(IInvocation invocation)
        {
            var args = invocation.Arguments.Where(x => x.GetType().IsUserDefinedClass());

            foreach (var argument in args)
            {
                Type itemType = argument.GetType();

                Type generic = typeof(IValidator<>);
                Type specific = generic.MakeGenericType(itemType);

                var _validator = _serviceProvider.GetService(specific) as IValidator;

                if (_validator == null || !_validator.CanValidateInstancesOfType(itemType))
                    return;

                var context = new ValidationContext<object>(argument);
                var validationResult = await _validator.ValidateAsync(context);

                if (validationResult.Errors.Any())
                {
                    List<ValidationMessage> validationFailures = new();

                    foreach (var error in validationResult.Errors)
                        validationFailures.Add(new ValidationMessage(char.ToLowerInvariant(error.PropertyName[0]) + error.PropertyName[1..], error.ErrorMessage));

                    throw new CustomValidationException(validationFailures);
                }
            }
        }

    }
}
