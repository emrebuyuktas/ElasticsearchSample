using ElasticsearchSample.API.Models;

namespace ElasticsearchSample.API.Dtos;

public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto ProductFeatureDto)
{
    public Product CreateProduct() =>
        new Product
        {
            Name=Name,
            Price= Price,
            Stock= Stock,
            Feature = ProductFeatureDto == null ? null : new ProductFeature
            {
                Width = (int)ProductFeatureDto.Width,
                Height = (int)ProductFeatureDto.Height,
                Color = (ColorEnum)ProductFeatureDto.Color
            }
        };
}