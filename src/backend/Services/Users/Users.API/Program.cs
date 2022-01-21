using Infrastructure.Core.Config;
using Infrastructure.Core.Extensions;
using Users.API.Mapping;
using Users.API.Services;
using Users.Domain.Services;
using Users.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(TamagotchiConfiguration.Prefix);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddDataAccess<UsersDataContext>(builder.Configuration.GetConnectionString("UsersDb"));

var app = builder.Build();
app.Logger.LogInformation(ConfigurationSerializer.Serialize(app.Configuration).ToString());

// Configure the HTTP request pipeline.
app.MapGrpcService<GrpcUsersService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
