using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticsearchSample.API.Extensions;

public static class ElasticsearchExtension
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var userName = configuration.GetSection("Elasticsearch")["UserName"]!;
        var password = configuration.GetSection("Elasticsearch")["Password"]!;
        var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elasticsearch")["Url"]!))
            .Authentication(new BasicAuthentication(userName,password));
        var client = new ElasticsearchClient(settings);
        services.AddSingleton(client);
    }
}