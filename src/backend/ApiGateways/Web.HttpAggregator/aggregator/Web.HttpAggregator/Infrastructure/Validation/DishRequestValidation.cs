using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation
{
    public class DishRequestValidation : AbstractValidator<DishRequest>
    {
        public DishRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}