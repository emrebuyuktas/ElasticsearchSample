namespace ElasticsearchSample.API.Dtos;

public class AddBlogDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public List<string> Tags { get; set; } = new();
}
