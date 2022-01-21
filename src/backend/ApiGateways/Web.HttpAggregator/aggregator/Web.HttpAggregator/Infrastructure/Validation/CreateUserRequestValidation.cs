using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation;

public class CreateUserRequestValidation : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidation()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Roles).NotEmpty();
    }
}