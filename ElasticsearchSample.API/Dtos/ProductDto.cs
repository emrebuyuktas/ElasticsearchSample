using ElasticsearchSample.API.Models;

namespace ElasticsearchSample.API.Dtos;

public record ProductDto(string id,string Name, decimal Price, int Stock, ProductFeatureDto? ProductFeatureDto)
{
    public static ProductDto FromProduct(Product product) =>
        new ProductDto(product.Id,
                       product.Name,
                                  product.Price,
                                             product.Stock,
                                                        product.Feature == null ? null : new ProductFeatureDto(product.Feature.Width, product.Feature.Height, product.Feature.Color)
                   );
}
