using Elasticsearch.Net;
using Nest;

namespace ElasticsearchSample.API.Extensions;

public static class ElasticsearchExtension
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elasticsearch")["Url"]!));
        var settings = new ConnectionSettings(pool);
        var client = new ElasticClient(settings);
        services.AddSingleton(client);
    }
}