using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation
{
    public class AuthenticateRequestValidation : AbstractValidator<AuthenticateRequest>
    {
        public AuthenticateRequestValidation()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}