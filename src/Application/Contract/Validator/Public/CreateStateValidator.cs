using Contract.Services.Public;
using Contract.Services.Public.DTOs.State;
using FluentValidation;

namespace Contract.Validator.Public;

public class CreateStateValidator : AbstractValidator<CreateState>
{
    private readonly IStateService _stateService;
    public CreateStateValidator(IStateService stateService)
    {
        _stateService=stateService;

        Include(new StateDtoValidator(_stateService));

        RuleFor(x => x.Code)
            .GreaterThan(0).WithMessage("باید بزرگتر از 0 باشد");
    }
}

