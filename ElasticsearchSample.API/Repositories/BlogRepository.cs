using Elastic.Clients.Elasticsearch;
using ElasticsearchSample.API.Models.BlogModels;

namespace ElasticsearchSample.API.Repositories;

public class BlogRepository
{
    private readonly ElasticsearchClient _client;
    private const string IndexName = "blog";

    public BlogRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task<Blog> SaveAsync(Blog blog)
    {

        var response = await _client.IndexAsync(blog, x => x.Index(IndexName));

        if (!response.IsValidResponse)
        {
            throw new Exception(response.DebugInformation);
        }

        blog.Id = response.Id;

        return blog;
    }

    public async Task<List<Blog>> SearchAsync(string searcText)
    {
        //Title, Content
        var response = await _client.SearchAsync<Blog>(x => x.Index(IndexName).Query(q => q.Bool(b =>
        b.Should(s => s.Match(m => m.Field(f => f.Content).Query(searcText)),
        s=>s.MatchBoolPrefix(p=>p.Field(f=>f.Title).Query(searcText))))));

        if (!response.IsValidResponse)
        {
            throw new Exception(response.DebugInformation);
        }

        return response.Documents.ToList();
    }
}
