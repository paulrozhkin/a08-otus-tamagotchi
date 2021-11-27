using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation
{
    public class MenuItemRequestValidation : AbstractValidator<MenuItemRequest>
    {
        public MenuItemRequestValidation()
        {
            RuleFor(x => x.DishId).NotEmpty();
            RuleFor(x => x.PriceRubles).GreaterThan(0);
        }
    }
}