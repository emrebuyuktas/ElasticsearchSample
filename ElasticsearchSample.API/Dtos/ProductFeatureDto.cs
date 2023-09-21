using ElasticsearchSample.API.Models;

namespace ElasticsearchSample.API.Dtos;

public record ProductFeatureDto(int? Width, int? Height, ColorEnum? Color)
{
}
