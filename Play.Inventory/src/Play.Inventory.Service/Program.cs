using Polly;
using Play.Common.Extensions;
using Play.Common.Settings;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entites;

var builder = WebApplication.CreateBuilder(args);

// Services: Dependencies
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services
    .AddMongoDatabase()
    .AddMongoRepository<InventoryItem>("inventoryitems");

var url = builder.Configuration.GetValue<string>("CatalogEndpoint:BaseUrl")!;
builder.Services.AddHttpClient<CatalogClient>(client =>
           {
               client.BaseAddress = new Uri(url);
           })
           .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
