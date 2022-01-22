using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Web.HttpAggregator.Config;
using Web.HttpAggregator.Services;

namespace Web.HttpAggregator.Infrastructure.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();

            var configSection = configuration.GetSection(AuthenticateOptions.Authenticate);
            services.Configure<AuthenticateOptions>(configSection);

            var secret = configuration[$"{AuthenticateOptions.Authenticate}:{nameof(AuthenticateOptions.Secret)}"];
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}