using Contract.Services.Public;
using Contract.Services.Public.DTOs.State;
using FluentValidation;

namespace Contract.Validator.Public;

public class UpdateStateValidator : AbstractValidator<UpdateState>
{
    private readonly IStateService _stateService;
    public UpdateStateValidator(IStateService stateService)
    {
        _stateService = stateService;

        Include(new StateDtoValidator(_stateService));
    }
}

