namespace ElasticsearchSample.API.Dtos;

public class EcommerceSearchDto
{
    public string? Category { get; set; }
    public string? Gender { get; set; }
    public DateTime? OrderDateStart { get; set; }
    public DateTime? OrderDateEnd { get; set; }
    public string? CustomerFullName { get; set; }
}
