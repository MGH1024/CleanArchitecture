using Contract.Services.Public;
using Contract.Services.Public.DTOs.State.Base;
using FluentValidation;

namespace Contract.Validator.Public
{
    public class StateDtoValidator : AbstractValidator<StateDto>
    {
        private readonly IStateService _stateService;

        public StateDtoValidator(IStateService stateService)
        {
            _stateService = stateService;

            RuleFor(a => a.Title)
                .NotEmpty().WithMessage("not empty")
                .MaximumLength(64).WithMessage("length error")
                .MustAsync(async (title, token) =>
                {
                    return await _stateService.IsStateRegistered(title);
                });

            RuleFor(x => x.Code)
                .GreaterThan(0).WithMessage("باید بزرگتر از 0 باشد");
        }
    }
}
