using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation;

public class RestaurantRequestValidation : AbstractValidator<RestaurantRequest>
{
    public RestaurantRequestValidation()
    {
        RuleFor(x => x.Latitude).NotEmpty();
        RuleFor(x => x.Longitude).NotEmpty();
    }
}