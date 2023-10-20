
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

var builder = WebApplication.CreateBuilder(args);


// Services: Configurations
Play.Catalog.Service.Configurations.MongoDbConfig.RegisterSerializers();

// Services: Dependencies
ServiceSettings serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddSingleton(serviceProvider =>
{
    var mongoDbSetting = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);
    return mongoClient.GetDatabase(serviceSettings.ServiceName);
});
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();

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
