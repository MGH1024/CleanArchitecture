using Contract.Services.Public.DTOs.State;
using FluentValidation;

namespace Contract.Validator.Public;

public class CreateStateValidator : AbstractValidator<CreateState>
{
    public CreateStateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Not null");
        RuleFor(x => x.Code)
            .GreaterThan(0).WithMessage("باید بزرگتر از 0 باشد");
    }
}

