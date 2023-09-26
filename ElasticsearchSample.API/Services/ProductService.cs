using Elastic.Clients.Elasticsearch;
using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace ElasticsearchSample.API.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {
        var response = await _productRepository.SaveAsync(request.CreateProduct());
        if (response == null) return ResponseDto<ProductDto>.Fail(new List<string> { "Product could not be saved." }, HttpStatusCode.InternalServerError);

        return ResponseDto<ProductDto>.Success(ProductDto.FromProduct(response), HttpStatusCode.Created);
    }

    public async Task<ResponseDto<IImmutableList<ProductDto>>> GetAllAsync()
    {
        var response = await _productRepository.GetAllAsync();

        var products= response.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature?.Width, x.Feature?.Height, x.Feature?.Color))).ToImmutableList();

        return ResponseDto<IImmutableList<ProductDto>>.Success(products, HttpStatusCode.OK);

    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {      var response = await _productRepository.GetByIdAsync(id);
           if (response == null) return ResponseDto<ProductDto>.Fail(new List<string> { "Product could not be found." }, HttpStatusCode.NotFound);
    
           return ResponseDto<ProductDto>.Success(ProductDto.FromProduct(response), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto product)
    {

        var response = await _productRepository.UpdateAsync(product.Id,product.CreateProduct());

        if (!response) return ResponseDto<bool>.Fail(new List<string> { "Product could not be updated." }, HttpStatusCode.InternalServerError);

        return ResponseDto<bool>.Success(response, HttpStatusCode.NoContent);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        var response = await _productRepository.DeleteAsync(id);

        if (!response.IsValidResponse && response.Result == Result.NotFound) return ResponseDto<bool>.Fail(new List<string> { "Product could not be found." }, HttpStatusCode.NotFound);


        if (!response.IsValidResponse)
        {
            response.TryGetOriginalException(out  Exception? ex);
            _logger.LogError(ex, ex.Message);
            return ResponseDto<bool>.Fail(new List<string> { "Product could not be deleted." }, HttpStatusCode.InternalServerError);
        }
        return ResponseDto<bool>.Success(response.IsSuccess(), HttpStatusCode.NoContent);
    }
}