using ElasticsearchSample.API.Dtos;
using ElasticsearchSample.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchSample.API.Controllers;


public class ProductsController : BaseControlller
{

    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveAsync(ProductCreateDto request)=> CreateActionResult(await _productService.SaveAsync(request));

    [HttpGet]
    public async Task<IActionResult> GetAllAsync() => CreateActionResult(await _productService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id) => CreateActionResult(await _productService.GetByIdAsync(id));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(ProductUpdateDto request) => CreateActionResult(await _productService.UpdateAsync(request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id) => CreateActionResult(await _productService.DeleteAsync(id));
}