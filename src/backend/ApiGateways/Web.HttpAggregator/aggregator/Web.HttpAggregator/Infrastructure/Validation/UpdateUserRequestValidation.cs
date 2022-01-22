using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation;

public class UpdateUserRequestValidation : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidation()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Roles).NotEmpty();
    }
}