using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchSample.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly BlogService _blogService;

    public BlogsController(BlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpPost("add-blog")]
    public async Task<IActionResult> SaveAsync(AddBlogDto blogCreateViewModel)
    {
        var result = await _blogService.SaveAsync(blogCreateViewModel);

        return Created(string.Empty, result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(string searchText)
    {
        var result = await _blogService.SearchAsync(searchText);

        return Ok(result);
    }
}
