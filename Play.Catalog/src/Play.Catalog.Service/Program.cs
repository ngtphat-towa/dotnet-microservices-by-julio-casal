
using MassTransit;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;
using Play.Common.Extensions;
using Play.Common.Settings;


var builder = WebApplication.CreateBuilder(args);

// Services: Dependencies
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services
    .AddMongoDatabase()
    .AddMongoRepository<Item>("items");

var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(rabbitMQSettings?.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings?.ServiceName, false));
    });
});

/// Services: Controllers
builder.Services.AddControllers(
    options => { options.SuppressAsyncSuffixInActionNames = false; }
);

/// Services: EndpointsApiExplorer
builder.Services.AddEndpointsApiExplorer();

/// Services: SwaggerGen
builder.Services.AddSwaggerGen(
    c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Play.Catalog.Service",
        Version = "v1"
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
