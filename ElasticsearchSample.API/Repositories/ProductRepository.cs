using ElasticsearchSample.API.Models;
using Nest;

namespace ElasticsearchSample.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _client;

    public ProductRepository(ElasticClient client)
    {
        _client = client;
    }

    public async Task<Product> SaveAsync(Product product)
    {

        product.Created = DateTime.Now;
        var response = await _client.IndexAsync(product, x => x.Index("Products"));
        if (!response.IsValid) return null;


        product.Id = response.Id;

        return product;
    }
}