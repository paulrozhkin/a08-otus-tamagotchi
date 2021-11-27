using FluentValidation;
using Web.HttpAggregator.Models;

namespace Web.HttpAggregator.Infrastructure.Validation
{
    public class TableRequestValidation : AbstractValidator<TableRequest>
    {
        public TableRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.NumberOfPlaces).GreaterThan(0);
        }
    }
}