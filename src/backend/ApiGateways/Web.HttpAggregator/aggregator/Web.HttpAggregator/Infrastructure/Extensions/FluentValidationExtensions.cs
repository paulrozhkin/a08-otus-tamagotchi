using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Web.HttpAggregator.Infrastructure.Validation;

namespace Web.HttpAggregator.Infrastructure.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services)
        {
            // Registration all validation in current assembly
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<DishRequestValidation>());
            return services;
        }
    }
}