using Elastic.Clients.Elasticsearch;
using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Models;
using System.Collections.Immutable;

namespace ElasticsearchSample.API.Repositories;

public class ProductRepository
{
    private readonly ElasticsearchClient _client;

    public ProductRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    private const string _indexName = "products";

    public async Task<Product> SaveAsync(Product product)
    {

        product.Created = DateTime.Now;
        //var response = await _client.IndexAsync(product, x => x.Index(_indexName).Id(Guid.NewGuid().ToString()));
        var response = await _client.IndexAsync(product, x => x.Index(_indexName));
        if (!response.IsValidResponse) return null;


        product.Id = response.Id;

        return product;
    }

    public async Task<IImmutableList<Product>> GetAllAsync()
    {
        var result=await _client.SearchAsync<Product>(x => x.Index(_indexName).Query(q=>q.MatchAll()));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        var result = await _client.GetAsync<Product>(id, x => x.Index(_indexName));
        if (!result.IsValidResponse) return null;

        result.Source.Id = result.Id;

        return result.Source;
    }

    public async Task<bool> UpdateAsync(string id,Product product)
    {
        var response = await _client.UpdateAsync<Product, Product>(_indexName,id,x=>
        x.Doc(product));
        return response.IsValidResponse;
    }


    public async Task<DeleteResponse> DeleteAsync(string id)
    {
        var response = await _client.DeleteAsync<Product>(id, x => x.Index(_indexName));
        return response;
    }
}