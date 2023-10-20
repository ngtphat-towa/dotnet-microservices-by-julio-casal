using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Extensions;
public static class Extension
{
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services)
    {
        // Services: Configurations
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        // Inject the services
        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>()!;
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSetting = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }
    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
    where T : IEntity
    {
        // Inject the services
        services.AddSingleton<IRepository<T>>(servicesProvider =>
        {
            var mongoDatabase = servicesProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(mongoDatabase!, collectionName);
        });
        return services;
    }
}