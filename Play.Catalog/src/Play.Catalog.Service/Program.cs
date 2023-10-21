
using Play.Catalog.Service.Entities;
using Play.Common.Extensions;
using Play.Common.Settings;
using Play.Common.MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Services: Dependencies
builder.Configuration
    .GetSection(nameof(ServiceSettings))
    .Get<ServiceSettings>();    

builder.Services
    .AddMongoDatabase()
    .AddMongoRepository<Item>("items")
    .AddMassTransitWithRabbitMQ();




/// Services: Controllers
builder.Services
    .AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    }
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
