using Contract.Services.Public.DTOs.State;
using FluentValidation;

namespace Contract.Validator.Public;

public class GetStateByIdValidator : AbstractValidator<GetStateById>
{
    public GetStateByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Not null")
            .GreaterThan(0).WithMessage("must greater than 1");
    }
}

