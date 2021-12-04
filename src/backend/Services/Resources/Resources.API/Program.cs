using Domain.Core.Models;
using Infrastructure.Core.Config;
using MongoDB.Bson.Serialization;
using Resources.API.Config;
using Resources.API.Mapping;
using Resources.API.Models;
using Resources.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(TamagotchiConfiguration.Prefix);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<ResourcesDatabaseOptions>(builder.Configuration.GetSection(ResourcesDatabaseOptions.ResourcesDatabase));
builder.Services.AddSingleton<IResourcesMetadataService, ResourcesMetadataMetadataService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
{
    cm.SetIsRootClass(true);
    cm.AutoMap();
    cm.MapIdMember(c => c.Id);
    cm.MapProperty(c => c.CreatedDate).SetIsRequired(true);
});

BsonClassMap.RegisterClassMap<ResourceMetadata>(cm =>
{
    cm.AutoMap();
    cm.MapProperty(c => c.ResourceName).SetIsRequired(true);
    cm.MapProperty(c => c.ResourceType).SetIsRequired(true);
});

var app = builder.Build();
app.Logger.LogInformation(ConfigurationSerializer.Serialize(app.Configuration).ToString());

// Configure the HTTP request pipeline.
app.MapGrpcService<GrpcResourcesService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
