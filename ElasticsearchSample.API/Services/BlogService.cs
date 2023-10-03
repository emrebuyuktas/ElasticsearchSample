using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Models.BlogModels;
using ElasticsearchSample.API.Repositories;

namespace ElasticsearchSample.API.Services;

public class BlogService
{
    private readonly BlogRepository _blogRepository;

    public BlogService(BlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<Blog> SaveAsync(AddBlogDto blogCreateViewModel)
    {
        var blog = new Blog
        {
            Title = blogCreateViewModel.Title,
            Content = blogCreateViewModel.Content,
            Tags = blogCreateViewModel.Tags,
            UserId = Guid.NewGuid(),
            Created = DateTime.Now
        };

        var crated = await _blogRepository.SaveAsync(blog);

        return crated;
    }

    public async Task<List<Blog>> SearchAsync(string searchText)
    {
        return await _blogRepository.SearchAsync(searchText);
    }
}
